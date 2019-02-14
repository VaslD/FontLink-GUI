using System;

namespace FontLinkSettings
{
    internal class FontLinkFallback
    {
        public String Typeface    { get; set; }
        public String Collection  { get; set; }
        public Int32  GDISize     { get; set; }
        public Int32  DirectXSize { get; set; }

        public override String ToString()
        {
            return $"{Typeface} ({Collection})";
        }

        public String GDISizeString     => $"GDI Size: {GDISize}";
        public String DirectXSizeString => $"Direct2D Size: {DirectXSize}";

        public String ExportString
        {
            get
            {
                if (GDISize == 0 || DirectXSize == 0)
                {
                    return $"{Collection},{Typeface}";
                }

                return $"{Collection},{Typeface},{GDISize},{DirectXSize}";
            }
        }
    }
}
