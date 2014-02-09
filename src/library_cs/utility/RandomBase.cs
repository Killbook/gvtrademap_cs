/*
 * Copyright (C) Rei HOBARA 2007
 * 
 * Name:
 *     RandomBase.cs
 * Class:
 *     Rei.Random.RandomBase
 * Purpose:
 *     A base class for random number generator.
 * Remark:
 * History:
 *     2007/10/6 initial release.
 * 
 */

using System;
using System.Diagnostics;

//namespace Rei.Random {
namespace Utility {

    /// <summary>
    /// �e��[�������W�F�l���[�^�[�p���N���X�B
    /// �h���N���X��NextUInt32����������K�v������܂��B
    /// </summary>
    public abstract class RandomBase {

        /// <summary>
        /// �h���N���X�ŕ����Ȃ�32bit�̋[�������𐶐�����K�v������܂��B
        /// </summary>
        public abstract UInt32 NextUInt32();

        /// <summary>
        /// ��������32bit�̋[���������擾���܂��B
        /// </summary>
        public virtual Int32 NextInt32() {
            return (Int32)NextUInt32();
        }

        /// <summary>
        /// �����Ȃ�64bit�̋[���������擾���܂��B
        /// </summary>
        public virtual UInt64 NextUInt64() {
            return ((UInt64)NextUInt32() << 32) | NextUInt32();
        }

        /// <summary>
        /// ��������64bit�̋[���������擾���܂��B
        /// </summary>
        public virtual Int64 NextInt64() {
            return ((Int64)NextUInt32() << 32) | NextUInt32();
        }

        /// <summary>
        /// �[��������𐶐����A�o�C�g�z��ɏ��Ɋi�[���܂��B
        /// </summary>
        public virtual void NextBytes( byte[] buffer ) {
            int i = 0;
            UInt32 r;
            while (i + 4 <= buffer.Length) {
                r = NextUInt32();
                buffer[i++] = (byte)r;
                buffer[i++] = (byte)(r >> 8);
                buffer[i++] = (byte)(r >> 16);
                buffer[i++] = (byte)(r >> 24);
            }
            if (i >= buffer.Length) return;
            r = NextUInt32();
            buffer[i++] = (byte)r;
            if (i >= buffer.Length) return;
            buffer[i++] = (byte)(r >> 8);
            if (i >= buffer.Length) return;
            buffer[i++] = (byte)(r >> 16);
        }
		
		/// <summary>
        /// [0,1)�̋[���������擾���܂��B
		/// �Ԃ����l��0���܂݂܂���1���܂݂܂���B
        /// </summary>
        public virtual double NextDouble()
		{
			return (1.0/4294967296.0) * NextUInt32();
		}

		/// <summary>
        /// [0,1]�̋[���������擾���܂��B
		/// �Ԃ����l��0�y��1���܂݂܂��B
        /// </summary>
        public virtual double NextDouble2()
		{
			return (1.0/4294967295.0) * NextUInt32();
		}
	
		/// <summary>
		/// 0�ȏ�̕����t���������擾���܂��B
        /// </summary>
		public virtual int Next()
		{
			return (Int32)NextUInt32();
		}

		/// <summary>
		/// �w�肵���ő�l��菬���� 0 �ȏ�̗�����Ԃ��܂��B
		/// �Ԃ����l�ɂ�0���܂݂܂���max_value���܂݂܂���B
        /// </summary>
		public virtual int Next(int max_value)
		{
			Trace.Assert(max_value >= 0, "RandomBase.Next()", "max_value �� 0 �ȏ�ɂ���K�v������܂��B ");
			return (int)(NextDouble() * max_value);
		}

		/// <summary>
		/// �w�肵���͈͓��̗�����Ԃ��܂��B
		/// �Ԃ����l�ɂ�min_value���܂݂܂���max_value���܂݂܂���B
        /// </summary>
		public virtual int Next(int min_value, int max_value)
		{
			Trace.Assert(max_value >= min_value, "RandomBase.Next()", "max_value �� min_value �ȏ�ɂ���K�v������܂��B");
			max_value	-= min_value;	// �K�����̒l
			return min_value + (int)(NextDouble() * max_value);
		}
	}
}
