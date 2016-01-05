using System;
using System.Text;
using System.Windows.Forms;

namespace innoGlue.innoGlue.Extensions
{
    public static class ExceptionExtensions
    {
        private static string FormatError(Exception e, string source = null)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(source))
            {
                builder.Append(source);
            }
            if (!string.IsNullOrEmpty(e.Message))
            {
                builder.AppendLine(e.Message);
            }
            else
            {
                builder.AppendLine(string.Format("[{0}]", e.GetType().Name));
            }
            if (e.InnerException != null)
            {
                return FormatError(e.InnerException, builder.ToString());
            }
            return builder.ToString();
        }

        public static void ShowError(this Exception e)
        {
            ShowError(e, IntPtr.Zero);
        }

        public static void ShowError(this Exception e, IntPtr windowHandle)
        {
            IWin32Window parentWin32 = null;
            if (windowHandle != IntPtr.Zero)
            {
                try
                {
                    var nw = new NativeWindow();
                    nw.AssignHandle(windowHandle);
                    parentWin32 = nw;
                }
                catch
                {
                    parentWin32 = null;
                }
            }
            if (parentWin32 != null)
            {
                MessageBox.Show(parentWin32, FormatError(e), "innoGlue internal error", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
            }
            else
            {
                MessageBox.Show(FormatError(e), "innoGlue internal error", MessageBoxButtons.OK,
                       MessageBoxIcon.Hand);
                
            } 
        }
    }
}
