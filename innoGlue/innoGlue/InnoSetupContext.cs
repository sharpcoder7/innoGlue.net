using System;
using innoGlue.innoGlue.Extensions;

namespace innoGlue.innoGlue
{
    public class InnoSetupContext
    {
        private readonly InnoSetupHandler _handler;

        internal InnoSetupContext(InnoSetupHandler innoSetupHandler)
        {
            if (innoSetupHandler == null)
            {
                throw new ArgumentNullException("innoSetupHandler");
            }
            this._handler = innoSetupHandler;
            InitEventHandlers();
        }

        #region inno setup wrapped functions

        public string ExpandConstant(string s)
        {
            string retval;
            _handler.ExpandContstant(s, out retval);
            return retval;
        }

        public int CreateCusomPage(BaseWizardPage customPage, 
            int afterId, string caption, string description)
        {
            return _handler.CreatePage(customPage.Handle, afterId, caption, description);
        }

        public void ExtractTemporaryFile(string fileName)
        {
            _handler.ExtractTemporaryFile(fileName);
        }

        public int ExtractTemporaryFiles(string pattern)
        {
            return _handler.ExtractTemporaryFiles(pattern);
        }

        #endregion

        #region events

        public event EventHandler SetupInitialize;
        public event EventHandler WizardInitialize;
        public event EventHandler SetupDeinitialize;
        public event EventHandler<PageChangedEventArgs> CurPageChanged;

        private void InitEventHandlers()
        {
            _handler.SetupInitialize += HandlerOnSetupInitialize;
            _handler.WizardInitialize += HandlerOnWizardInitialize;
            _handler.SetupDeinitialize += HandlerOnSetupDeinitialize;
            _handler.CurPageChanged += HandlerOnCurPageChanged;
        }

        private void HandlerOnCurPageChanged(object sender, PageChangedEventArgs args)
        {
            FireCurPageChanged(args);
        }

        private void HandlerOnSetupDeinitialize(object sender, EventArgs eventArgs)
        {
            FireSetupDeinitialize();
        }

        private void HandlerOnWizardInitialize(object sender, EventArgs eventArgs)
        {
            FireWizardInitialize();
        }

        private void HandlerOnSetupInitialize(object sender, EventArgs eventArgs)
        {
            FireSetupInitialize();
        }

        private void FireCurPageChanged(PageChangedEventArgs args)
        {
            try
            {
                var handler = CurPageChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private void FireSetupDeinitialize()
        {
            try
            {
                var handler = SetupDeinitialize;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private void FireSetupInitialize()
        {
            try
            {
                var handler = SetupInitialize;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private void FireWizardInitialize()
        {
            try
            {
                var handler = WizardInitialize;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        #endregion
    }
}
