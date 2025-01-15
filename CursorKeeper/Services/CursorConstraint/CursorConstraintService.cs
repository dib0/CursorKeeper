using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CursorKeeper.Services.CursorConstraint
{
    public sealed class CursorConstraintService : ICursorConstraintService
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_MOUSEMOVE = 0x200;
        private const int HC_ACTION = 0;
        private const int EDGE_PADDING = 2;

        private IntPtr _hookHandle = IntPtr.Zero;
        private readonly MouseHookDelegate _hookDelegate;
        private readonly Screen _primaryScreen;
        private Point _lastValidPosition;
        private bool _disposed;

        public bool IsEnabled { get; private set; }

        private delegate IntPtr MouseHookDelegate(int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, MouseHookDelegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public CursorConstraintService()
        {
            _hookDelegate = MouseHookCallback;
            _primaryScreen = Screen.PrimaryScreen ?? throw new InvalidOperationException("No primary screen found");
            _lastValidPosition = Cursor.Position;
        }

        public void EnableConstraints()
        {
            if (IsEnabled) return;

            _hookHandle = SetWindowsHookEx(WH_MOUSE_LL, _hookDelegate, IntPtr.Zero, 0);
            if (_hookHandle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to set mouse hook");

            IsEnabled = true;
        }

        public void DisableConstraints()
        {
            if (!IsEnabled || _hookHandle == IntPtr.Zero) return;

            UnhookWindowsHookEx(_hookHandle);
            _hookHandle = IntPtr.Zero;
            IsEnabled = false;
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam)
        {
            if (nCode == HC_ACTION && wParam == (IntPtr)WM_MOUSEMOVE)
            {
                var currentPos = new Point(lParam.pt.x, lParam.pt.y);

                if (!IsPositionInPrimaryScreen(currentPos))
                {
                    var newPos = CalculateValidPosition(currentPos);
                    SetCursorPos(newPos.X, newPos.Y);
                    _lastValidPosition = newPos;
                    return (IntPtr)1;
                }

                _lastValidPosition = currentPos;
            }

            return CallNextHookEx(_hookHandle, nCode, wParam, ref lParam);
        }

        private bool IsPositionInPrimaryScreen(Point position)
        {
            var paddedBounds = new Rectangle(
                _primaryScreen.Bounds.Left + EDGE_PADDING,
                _primaryScreen.Bounds.Top + EDGE_PADDING,
                _primaryScreen.Bounds.Width - (2 * EDGE_PADDING),
                _primaryScreen.Bounds.Height - (2 * EDGE_PADDING)
            );

            return paddedBounds.Contains(position);
        }

        private Point CalculateValidPosition(Point currentPos) =>
            new(
                    Math.Max(_primaryScreen.Bounds.Left + EDGE_PADDING,
                    Math.Min(currentPos.X, _primaryScreen.Bounds.Right - EDGE_PADDING)),
                    Math.Max(_primaryScreen.Bounds.Top + EDGE_PADDING,
                    Math.Min(currentPos.Y, _primaryScreen.Bounds.Bottom - EDGE_PADDING))
            );

        public void Dispose()
        {
            if (_disposed) return;

            DisableConstraints();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
