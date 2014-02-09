/*-------------------------------------------------------------------------
 �X�v���C�g�`��p�V�F�[�_
 �n�}�̃I�t�Z�b�g�ƃX�P�[���ɑΉ��������̂��܂܂��
 vs_1_1 ps_1_1 �p
---------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------
 params
---------------------------------------------------------------------------*/
// �r���[�|�[�g���
// 1/ViewportSize�̓v���V�F�[�_�����߂�
float2	ViewportSize;	// {width, height }

// �n�}�̃I�t�Z�b�g
float2	MapOffset;
// �n�}�̃X�P�[��
float	MapScale;
// �؂������X�v���C�g��`�Ɋ|������X�P�[��
float2	GlobalScale;

// �e�N�X�`���T���v���ݒ�
texture	Texture;

/*-------------------------------------------------------------------------
 
---------------------------------------------------------------------------*/
sampler TextureSampler = sampler_state
{
	Texture = (Texture);
};

/*-------------------------------------------------------------------------
 �X�N���[�����W����r���[�|�[�g���W�֕ϊ�
---------------------------------------------------------------------------*/
float2 ScreenToViewport(float2 screenPos)
{
	// DirectX�̃X�N���[�����W��0.5�s�N�Z���̂���𒼂�
	screenPos		-= 0.5f;

	float2 result	= screenPos.xy / ViewportSize.xy * 2 - 1;
	result.y		= -result.y;
	return result;
}

/*-------------------------------------------------------------------------
 ��]
---------------------------------------------------------------------------*/
float2 RotationPosition(float2 p, float angle_rad)
{
	float cs = cos(angle_rad);
	float sn = sin(angle_rad);
	return mul(p, float2x2(cs, sn, -sn, cs));
}

/*-------------------------------------------------------------------------
 ���ʂ̃s�N�Z���V�F�[�_�[
---------------------------------------------------------------------------*/
void PS( inout float4 color : COLOR0, float2 texCoord : TEXCOORD0 )
{
    color *= tex2D(TextureSampler, texCoord);
}

/*-------------------------------------------------------------------------
 �X�N���[�����W����r���[�|�[�g���W�ւ̕ϊ��݂̂̃V�F�[�_
---------------------------------------------------------------------------*/
void TransformedVS(
	float3 position : POSITION0,
	float2 texCoord : TEXCOORD0,
	float4 color    : COLOR0,

	uniform	bool	b_with_offset,

	out float4 outputPosition : POSITION0,
	out float2 outputTexCoord : TEXCOORD0,
	out float4 outputColor    : COLOR0 )
{
	float2	pos;

	if(b_with_offset){
		// �O���[�o���p�����[�^�Ή���
		pos		= (position.xy + MapOffset) * MapScale;		// �n�}�̃I�t�Z�b�g�ƃX�P�[�����l��
		pos		= floor(pos);	// �������؂�̂�
	}else{
		// ���̂܂�
		pos		= position.xy;
	}

	// �X�N���[�����W����r���[�|�[�g���W�ɕϊ�
	pos		= ScreenToViewport(pos);
	outputPosition	= float4(pos.x, pos.y, position.z, 1);

	// ���̃p�����[�^�[�͂��̂܂�
	outputTexCoord	= texCoord;
	outputColor		= color;
}

/*-------------------------------------------------------------------------
 technique
 ���W�ϊ���̃X�v���C�g�`��p
---------------------------------------------------------------------------*/
technique Transformed
{
	pass Pass1
	{
		VertexShader	= compile vs_1_1 TransformedVS(false);
		PixelShader		= compile ps_1_1 PS();
	}
}

/*-------------------------------------------------------------------------
 technique
 ���W�ϊ���̃X�v���C�g�`��p

 MapOffset
 MapScale
 �ɑΉ�
 GlobalScale
 �ɂ͖��Ή��Ȃ̂Œ���
---------------------------------------------------------------------------*/
technique TransformedWithGlobalParams
{
	pass Pass1
	{
		VertexShader	= compile vs_1_1 TransformedVS(true);
		PixelShader		= compile ps_1_1 PS();
	}
}

/*-------------------------------------------------------------------------
 �ʏ�̃X�v���C�g
 ��]
 �g��
---------------------------------------------------------------------------*/
void SpriteVertexVS(
	float3 position	: POSITION0,
	float2 texCoord	: TEXCOORD0,
	float4 color	: COLOR0,
	float4 param	: TEXCOORD1,	// offset1.x, offset1.y, offset2.x, offset2.y
									// offset1 = ��`�؂�o���̃I�t�Z�b�g
									// offset2 = ���̑��̎��R�ȃI�t�Z�b�g
									// offset2�͒P���ɉ��Z�����
	float3 param2	: TEXCOORD2,	// scale.x, scale.y, angle_rad

	uniform	bool	b_with_offset,

	out float4 outputPosition : POSITION0,
	out float2 outputTexCoord : TEXCOORD0,
	out float4 outputColor    : COLOR0 )
{
	float2	pos, offset;

	if(b_with_offset){
		// �O���[�o���p�����[�^�ɉe������X�v���C�g
		pos		= (position.xy + MapOffset) * MapScale;		// �n�}�̃I�t�Z�b�g�ƃX�P�[�����l��
		pos		= floor(pos);	// �������؂�̂�
		offset	= (RotationPosition(param.xy, param2.z) * param2.xy) * GlobalScale.xy;
	}else{
		// �ʏ�̃X�v���C�g
		pos		= floor(position.xy);
		offset	= (RotationPosition(param.xy, param2.z) * param2.xy);
	}
	pos		= pos + param.zw + offset;

	// �X�N���[�����W����r���[�|�[�g���W�ɕϊ�
	pos				= ScreenToViewport(pos);
	outputPosition	= float4(pos.x, pos.y, position.z, 1);

	// ���̃p�����[�^�[�͂��̂܂�
	outputTexCoord	= texCoord;
	outputColor		= color;
}

/*-------------------------------------------------------------------------
 technique
 �ʏ�̃X�v���C�g
---------------------------------------------------------------------------*/
technique Sprite
{
	pass Pass1
	{
		VertexShader	= compile vs_1_1 SpriteVertexVS(false);
		PixelShader		= compile ps_1_1 PS();
	}
}

/*-------------------------------------------------------------------------
 technique
 �ʏ�̃X�v���C�g
 �I�t�Z�b�g�ƃX�P�[��
 ��`�ɑ΂���O���[�o���X�P�[���ɑΉ���
---------------------------------------------------------------------------*/
technique SpriteWithGlobalParams
{
	pass Pass1
	{
		VertexShader	= compile vs_1_1 SpriteVertexVS(true);
		PixelShader		= compile ps_1_1 PS();
	}
}
