using System;
using System.Drawing;
using System.Windows.Forms;
using CursorKeeper.Services.CursorConstraint;

namespace CursorKeeper
{
    public sealed class CursorKeeperContext : ApplicationContext, IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly ICursorConstraintService _cursorService;
        private readonly ToolStripMenuItem _enableMenuItem;
        private bool _disposed;

        public CursorKeeperContext()
        {
            (_trayIcon, _enableMenuItem) = InitializeComponents();
            _cursorService = new CursorConstraintService();
            _cursorService.EnableConstraints(); // Start with constraints enabled
        }

        private (NotifyIcon icon, ToolStripMenuItem menuItem) InitializeComponents()
        {
            // Create context menu
            var contextMenu = new ContextMenuStrip();
            var enableMenuItem = new ToolStripMenuItem("Disable CursorKeeper", null, ToggleConstraints);
            var exitMenuItem = new ToolStripMenuItem("Exit", null, Exit);

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                enableMenuItem,
                new ToolStripSeparator(),
                exitMenuItem
            });

            // Create tray icon
            var icon = new NotifyIcon
            {
                Icon = CreateCursorIcon(),
                ContextMenuStrip = contextMenu,
                Text = "CursorKeeper - Active",
                Visible = true
            };

            icon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    icon.ContextMenuStrip.Show(Cursor.Position);
            };

            return (icon, enableMenuItem);
        }

        private static Icon CreateCursorIcon()
        {
            using var stream = typeof(CursorKeeperContext).Assembly
                .GetManifestResourceStream("CursorKeeper.Resources.tray-icon16.png");
            if (stream == null)
                throw new InvalidOperationException("Could not load tray icon resource");

            using var bitmap = new Bitmap(stream);
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private void ToggleConstraints(object? sender, EventArgs e)
        {
            if (_cursorService.IsEnabled)
            {
                _cursorService.DisableConstraints();
                _enableMenuItem.Text = "Enable CursorKeeper";
                _trayIcon.Text = "CursorKeeper - Inactive";
            }
            else
            {
                _cursorService.EnableConstraints();
                _enableMenuItem.Text = "Disable CursorKeeper";
                _trayIcon.Text = "CursorKeeper - Active";
            }
        }

        private void Exit(object? sender, EventArgs e)
        {
            Dispose();
            Application.Exit();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            _cursorService.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
