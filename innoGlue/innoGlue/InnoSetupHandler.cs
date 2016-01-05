using System;
using System.Runtime.InteropServices;

namespace innoGlue.innoGlue
{
    public delegate void ExtractTemporaryFileProc([MarshalAs(UnmanagedType.BStr)]string fileName);
    public delegate int ExtractTemporaryFilesFunc([MarshalAs(UnmanagedType.BStr)]string pattern);
    public delegate int CreatePage(IntPtr wndHandle, int afterId, 
        [MarshalAs(UnmanagedType.BStr)]string caption, 
        [MarshalAs(UnmanagedType.BStr)]string description);

    public delegate void ExpandContstant([MarshalAs(UnmanagedType.BStr)] string s,
        [MarshalAs(UnmanagedType.BStr)]out string result);

    public class PageChangedEventArgs : EventArgs
    {
        public int CurPageId { set; get; }

        public PageChangedEventArgs(int curPageId)
        {
            CurPageId = curPageId;
        }

        public PageChangedEventArgs()
        {
        }
    }

    internal class InnoSetupHandler
    {
        public ExtractTemporaryFileProc ExtractTemporaryFile { internal set; get; }
        public ExtractTemporaryFilesFunc ExtractTemporaryFiles { internal set; get; }
        public CreatePage CreatePage { internal set; get; }
        public ExpandContstant ExpandContstant { internal set; get; }

        public event EventHandler SetupInitialize;
        public event EventHandler WizardInitialize;
        public event EventHandler SetupDeinitialize;
        public event EventHandler<PageChangedEventArgs> CurPageChanged;

        public void FireCurPageChanged(int curPageId)
        {
            var handler = CurPageChanged;
            if (handler != null)
            {
                handler(this, new PageChangedEventArgs(curPageId));
            }
        }

        public void FireSetupDeinitialize()
        {
            var handler = SetupDeinitialize;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void FireSetupInitialize()
        {
            var handler = SetupInitialize;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void FireWizardInitialize()
        {
            var handler = WizardInitialize;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
