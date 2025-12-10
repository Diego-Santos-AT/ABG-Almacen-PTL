using System.Runtime.InteropServices;

namespace ABGAlmacenPTL.Classes
{
    /// <summary>
    /// Class for manipulating API memory blocks
    /// Migrado desde VB6 cMemory.cls
    /// Author: Steve McMahon
    /// Date: 24 May 1998
    /// </summary>
    public class Memory : IDisposable
    {
        #if WINDOWS
        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        private static extern uint GlobalCompact(uint dwMinFree);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalReAlloc(IntPtr hMem, UIntPtr dwBytes, uint uFlags);

        [DllImport("kernel32.dll")]
        private static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);
        #endif

        public enum MemoryFlags : uint
        {
            GMEM_DDESHARE = 0x2000,
            GMEM_DISCARDABLE = 0x100,
            GMEM_DISCARDED = 0x4000,
            GMEM_INVALID_HANDLE = 0x8000,
            GMEM_FIXED = 0x0,
            GMEM_LOCKCOUNT = 0xFF,
            GMEM_MODIFY = 0x80,
            GMEM_MOVEABLE = 0x2,
            GMEM_NODISCARD = 0x20,
            GMEM_NOCOMPACT = 0x10,
            GMEM_NOT_BANKED = 0x1000,
            GMEM_LOWER = 0x1000, // GMEM_NOT_BANKED
            GMEM_NOTIFY = 0x4000,
            GMEM_SHARE = 0x2000,
            GMEM_VALID_FLAGS = 0x7F72,
            GMEM_ZEROINIT = 0x40,
            GPTR = 0x40 // GMEM_FIXED | GMEM_ZEROINIT
        }

        private IntPtr m_hMem = IntPtr.Zero;
        private IntPtr m_lPtr = IntPtr.Zero;
        private bool disposed = false;

        public IntPtr Handle
        {
            get => m_hMem;
            set
            {
                if (m_hMem != IntPtr.Zero)
                {
                    FreeMemory();
                }
                m_hMem = value;
            }
        }

        public IntPtr Pointer
        {
            get
            {
                #if WINDOWS
                if (m_hMem != IntPtr.Zero)
                {
                    if (m_lPtr == IntPtr.Zero)
                    {
                        LockMemory();
                    }
                    return m_lPtr;
                }
                #endif
                return IntPtr.Zero;
            }
        }

        public long Size
        {
            get
            {
                #if WINDOWS
                if (m_hMem != IntPtr.Zero)
                {
                    return (long)GlobalSize(m_hMem);
                }
                #endif
                return 0;
            }
        }

        public bool AllocateMemory(long lSize, MemoryFlags dwFlags = MemoryFlags.GPTR)
        {
            #if WINDOWS
            FreeMemory();
            m_hMem = GlobalAlloc((uint)dwFlags, (UIntPtr)lSize);
            return m_hMem != IntPtr.Zero;
            #else
            // En Android, usar memoria managed de .NET
            return false;
            #endif
        }

        public bool LockMemory()
        {
            #if WINDOWS
            if (m_hMem != IntPtr.Zero)
            {
                if (m_lPtr == IntPtr.Zero)
                {
                    m_lPtr = GlobalLock(m_hMem);
                    return m_lPtr != IntPtr.Zero;
                }
            }
            #endif
            return false;
        }

        public void UnlockMemory()
        {
            #if WINDOWS
            if (m_hMem != IntPtr.Zero)
            {
                if (m_lPtr != IntPtr.Zero)
                {
                    GlobalUnlock(m_hMem);
                    m_lPtr = IntPtr.Zero;
                }
            }
            #endif
        }

        public void FreeMemory()
        {
            #if WINDOWS
            if (m_hMem != IntPtr.Zero)
            {
                UnlockMemory();
                GlobalFree(m_hMem);
            }
            m_hMem = IntPtr.Zero;
            #endif
        }

        public void ReleaseDontFreeMemory()
        {
            // For GMEM_DDESHARE operations...
            UnlockMemory();
            m_hMem = IntPtr.Zero;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Liberar recursos managed
                }
                
                // Liberar recursos unmanaged
                FreeMemory();
                
                disposed = true;
            }
        }

        ~Memory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
