using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using innoGlue.innoGlue.Extensions;
using RGiesecke.DllExport;

namespace innoGlue.innoGlue
{
    public static class InnoGlue
    {
        [ThreadStatic] public static InnoSetupContext Context;

        private static readonly InnoSetupHandler Handler;

        static InnoGlue()
        {
            Handler = new InnoSetupHandler();
            Context = new InnoSetupContext(Handler);     
        }

        private static void CallStartup()
        {
            try
            {
                var starterType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type => type.IsClass && !type.IsAbstract &&
                                        type.Name == "Startup");

                if (starterType == null)
                {
                    throw new Exception("unable to find `Startup` class");
                }

                var startMethod = starterType.GetMethod("InitializeSetup");
                if (startMethod == null)
                {
                    throw new Exception("unable to find `InitializeSetup` method in `Startup` class");
                }

                var starter = Activator.CreateInstance(starterType);
                startMethod.Invoke(starter, new object[0]);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            
        }

        [DllExport("SetupInitializeCallback", CallingConvention.StdCall)]
        private static void SetupInitializeCallback()
        {
            CallStartup();
        }

        [DllExport("CurPageChangedCallback", CallingConvention.StdCall)]
        private static void CurPageChangedCallback(int curPageId)
        {
            Handler.FireCurPageChanged(curPageId);
        }

        [DllExport("SetupDeinitializeCallback", CallingConvention.StdCall)]
        private static void SetupDeinitializeCallback()
        {
            Handler.FireSetupDeinitialize();
        }

        [DllExport("WizardInitializeCallback", CallingConvention.StdCall)]
        private static void WizardInitializeCallback()
        {
            Handler.FireWizardInitialize();
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InnoCallbacksPointers
        {
            [FieldOffset(0)]
            public uint ExpandConstant;
            [FieldOffset(4)]
            public uint ExtractTemporaryFile;
            [FieldOffset(8)]
            public uint ExtractTemporaryFiles;
            [FieldOffset(12)]
            public uint CreatePage;
        }

        [DllExport("RegisterCallbacks", CallingConvention.StdCall)]
        private static void RegisterCallbacks(int pointer)
        {
            var pointers = (InnoCallbacksPointers)Marshal.PtrToStructure(new IntPtr(pointer), 
                typeof (InnoCallbacksPointers));

            Handler.ExtractTemporaryFile =
                (ExtractTemporaryFileProc) CreateDelegate<ExtractTemporaryFileProc>
                ((int)pointers.ExtractTemporaryFile);
            Handler.ExtractTemporaryFiles =
                (ExtractTemporaryFilesFunc)CreateDelegate<ExtractTemporaryFilesFunc>
                ((int)pointers.ExtractTemporaryFiles);
            Handler.CreatePage =
                (CreatePage)CreateDelegate<CreatePage>
                ((int)pointers.CreatePage);
            Handler.ExpandContstant =
                (ExpandContstant) CreateDelegate<ExpandContstant>
                    ((int) pointers.ExpandConstant);

        }

        private static Delegate CreateDelegate<T>(int pointer)
        {
            return Marshal.GetDelegateForFunctionPointer(
                    new IntPtr(pointer), typeof(T));
        }
    }
}
