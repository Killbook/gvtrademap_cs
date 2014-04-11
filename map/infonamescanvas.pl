#!/bin/perl

die "Usage: $0 cityname [fontsize]\n" unless @ARGV;

$name = $ARGV[0];
$point = $ARGV[1] || 8;

print <<__HTML__;
<!DOCTYPE html>
<html>
 <head><meta charset="utf-8"></head>
 <body style="background:gray">
  <canvas id="a_canvas" width="300" height="300"></canvas>
  <script type="text/javascript">
   var canvas = document.getElementById("a_canvas");
   var context = canvas.getContext("2d");
   var text = "$name";
   context.textBaseline = 'top';

   context.strokeStyle = 'white';
   context.font = "${point}pt Meiryo";
   context.lineWidth = 5.0;
   context.strokeText(text, 4, 4);

   context.fillStyle = 'black';
   context.fillText(text, 4, 4);
  </script>
 </body>
</html>
__HTML__

