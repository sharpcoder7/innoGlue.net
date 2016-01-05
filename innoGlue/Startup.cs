using System;
using innoGlue.innoGlue;

namespace innoGlue
{
    /// <summary>
    /// Sample of startup class.
    /// </summary>
    public class Startup
    {
        private readonly CustomPageTest _testPage = new CustomPageTest();

        /// <summary>
        /// This method MUST exists
        /// It will be called by innoGlue by InitializeSetup (InnoSetup runtime)
        /// </summary>
        public void InitializeSetup()
        {
            InnoGlue.Context.WizardInitialize += ContextOnWizardInitialize;
            InnoGlue.Context.CurPageChanged += ContextOnCurPageChanged;
        }

        private void ContextOnCurPageChanged(object sender, PageChangedEventArgs args)
        {
            if (!(_testPage.Tag is int))
            {
                return;
            }
            int page = args.CurPageId;
            if (page == (int) _testPage.Tag)
            {
                _testPage.Show();
            }
            else
            {
                _testPage.Hide();
            }
        }

        private void ContextOnWizardInitialize(object sender, EventArgs eventArgs)
        {
            _testPage.Tag = InnoGlue.Context.CreateCusomPage(_testPage, InnoConstants.wpSelectDir, 
                "WinForms page title", 
                "WinForms page description");
        }
    }
}
