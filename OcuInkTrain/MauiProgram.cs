﻿using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using OcuInkTrain.Handlers;
using OcuInkTrain.Utilities;
using OcuInkTrain.Views.AdvanceDrawingView;

namespace OcuInkTrain
{
    /// <summary>
    /// The main entry point for the Maui application.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates and configures the Maui application.
        /// </summary>
        /// <returns>The configured Maui application.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windowsLifecycleBuilder =>
                    {
                        windowsLifecycleBuilder.OnWindowCreated(window =>
                        {
                            window.Title = "OcuInk Train";
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                            var titleBar = appWindow.TitleBar;
                            titleBar.ExtendsContentIntoTitleBar = true;
                            WinApi.ShowWindow(handle, WinApi.SW_MAXIMIZE);
                        })
                        .OnClosed((window, args) =>
                        {
                            if (window.Title == "OcuInk Train" && OcuInkTrain.Utilities.SessionContext.CameraWin is not null)
                                Application.Current?.CloseWindow(OcuInkTrain.Utilities.SessionContext.CameraWin);
                        });
                    });
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<AdvancedDrawingView, AdvancedDrawingViewHandler>();
            });

            return builder.Build();
        }
    }
}
