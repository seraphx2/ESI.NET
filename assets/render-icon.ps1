# Renders assets/icon.png (128x128) from the same geometry as icon.svg.
# GDI+ can't parse SVG, so the shapes are redrawn here. Keep in sync with icon.svg.
Add-Type -AssemblyName System.Drawing

$S = 512                      # supersample, then downscale
$big = [System.Drawing.Bitmap]::new($S, $S)
$g = [System.Drawing.Graphics]::FromImage($big)
$g.SmoothingMode     = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.InterpolationMode  = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$g.Clear([System.Drawing.Color]::Transparent)

function P($x, $y) { [System.Drawing.PointF]::new($x / 128 * $S, $y / 128 * $S) }
function Hex($cx, $cy, $r) {
  $pts = @()
  foreach ($a in 0..5) {
    $rad = [Math]::PI / 180 * (60 * $a - 90)
    $pts += P ($cx + $r * [Math]::Cos($rad)) ($cy + $r * [Math]::Sin($rad))
  }
  , $pts
}

# rounded-rect background with a vertical gradient, clipped to the rounded corners
$rr = 24 / 128 * $S
$rect = [System.Drawing.RectangleF]::new(0, 0, $S, $S)
$path = [System.Drawing.Drawing2D.GraphicsPath]::new()
$path.AddArc(0, 0, $rr, $rr, 180, 90)
$path.AddArc($S - $rr, 0, $rr, $rr, 270, 90)
$path.AddArc($S - $rr, $S - $rr, $rr, $rr, 0, 90)
$path.AddArc(0, $S - $rr, $rr, $rr, 90, 90)
$path.CloseFigure()
$g.SetClip($path)
$grad = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
  $rect, [System.Drawing.ColorTranslator]::FromHtml('#123253'),
  [System.Drawing.ColorTranslator]::FromHtml('#08131F'), 90)
$g.FillRectangle($grad, $rect)

$cyan  = [System.Drawing.ColorTranslator]::FromHtml('#37C6E8')
$lite  = [System.Drawing.ColorTranslator]::FromHtml('#7CE3F6')
$core  = [System.Drawing.ColorTranslator]::FromHtml('#8FE9F8')

# outer aperture
$outer = Hex 64 64 42
$pen = [System.Drawing.Pen]::new($cyan, 7 / 128 * $S)
$pen.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
$g.DrawPolygon($pen, [System.Drawing.PointF[]]$outer)

# vertex nodes
$nb = [System.Drawing.SolidBrush]::new($cyan)
foreach ($pt in $outer) {
  $rad = 5.5 / 128 * $S
  $g.FillEllipse($nb, $pt.X - $rad, $pt.Y - $rad, $rad * 2, $rad * 2)
}

# inner core hex
$inner = Hex 64 64 20
$fillIn = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(40, $cyan))
$g.FillPolygon($fillIn, [System.Drawing.PointF[]]$inner)
$penIn = [System.Drawing.Pen]::new($lite, 3 / 128 * $S)
$penIn.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
$g.DrawPolygon($penIn, [System.Drawing.PointF[]]$inner)

# core dot
$cb = [System.Drawing.SolidBrush]::new($core)
$cr = 9 / 128 * $S
$g.FillEllipse($cb, $S / 2 - $cr, $S / 2 - $cr, $cr * 2, $cr * 2)

$g.Dispose()

$out = [System.Drawing.Bitmap]::new(128, 128)
$og = [System.Drawing.Graphics]::FromImage($out)
$og.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$og.SmoothingMode     = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$og.DrawImage($big, 0, 0, 128, 128)
$og.Dispose()

$dest = Join-Path $PSScriptRoot 'icon.png'
$out.Save($dest, [System.Drawing.Imaging.ImageFormat]::Png)
Write-Output "wrote $dest"
