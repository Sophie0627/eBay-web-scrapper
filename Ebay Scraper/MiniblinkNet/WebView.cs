using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace Kyozy.MiniblinkNet
{
    public class WebView:IDisposable
    {
        private IntPtr m_WebView;
        private IntPtr m_hWnd;
        private IntPtr m_OldProc;
        private wkePaintUpdatedCallback m_wkePaintUpdatedCallback;
        private WndProcCallback m_WndProcCallback;


        #region 
        private wkeTitleChangedCallback m_wkeTitleChangedCallback;
        private wkeMouseOverUrlChangedCallback m_wkeMouseOverUrlChangedCallback;
        private wkeURLChangedCallback2 m_wkeURLChangedCallback2;
        private wkeAlertBoxCallback m_wkeAlertBoxCallback;
        private wkeConfirmBoxCallback m_wkeConfirmBoxCallback;
        private wkePromptBoxCallback m_wkePromptBoxCallback;
        private wkeNavigationCallback m_wkeNavigationCallback;
        private wkeCreateViewCallback m_wkeCreateViewCallback;
        private wkeDocumentReady2Callback m_wkeDocumentReadyCallback;
        private wkeLoadingFinishCallback m_wkeLoadingFinishCallback;
        private wkeDownloadCallback m_wkeDownloadCallback;
        private wkeConsoleCallback m_wkeConsoleCallback;
        private wkeLoadUrlBeginCallback m_wkeLoadUrlBeginCallback;
        private wkeLoadUrlEndCallback m_wkeLoadUrlEndCallback;
        private wkeDidCreateScriptContextCallback m_wkeDidCreateScriptContextCallback;
        private wkeWillReleaseScriptContextCallback m_wkeWillReleaseScriptContextCallback;
        private wkeNetResponseCallback m_wkeNetResponseCallback;
        private wkeWillMediaLoadCallback m_wkeWillMediaLoadCallback;
        private wkeOnOtherLoadCallback m_wkeOnOtherLoadCallback;

        private EventHandler<TitleChangeEventArgs> m_titleChangeHandler = null;
        private EventHandler<MouseOverUrlChangedEventArgs> m_mouseOverUrlChangedHandler = null;
        private EventHandler<UrlChangeEventArgs> m_urlChangeHandler = null;
        private EventHandler<AlertBoxEventArgs> m_alertBoxHandler = null;
        private EventHandler<ConfirmBoxEventArgs> m_confirmBoxHandler = null;
        private EventHandler<PromptBoxEventArgs> m_promptBoxHandler = null;
        private EventHandler<NavigateEventArgs> m_navigateHandler = null;
        private EventHandler<CreateViewEventArgs> m_createViewHandler = null;
        private EventHandler<DocumentReadyEventArgs> m_documentReadyHandler = null;
        private EventHandler<LoadingFinishEventArgs> m_loadingFinishHandler = null;
        private EventHandler<DownloadEventArgs> m_downloadHandler = null;
        private EventHandler<ConsoleEventArgs> m_consoleHandler = null;
        private EventHandler<LoadUrlBeginEventArgs> m_loadUrlBeginHandler = null;
        private EventHandler<LoadUrlEndEventArgs> m_loadUrlEndHandler = null;
        private EventHandler<DidCreateScriptContextEventArgs> m_didCreateScriptContextHandler = null;
        private EventHandler<WillReleaseScriptContextEventArgs> m_willReleaseScriptContextHandler = null;
        private EventHandler<NetResponseEventArgs> m_netResponseHandler = null;
        private EventHandler<WillMediaLoadEventArgs> m_willMediaLoadHandler = null;
        private EventHandler<OtherLoadEventArgs> m_OtherLoadHandler = null;

       
        public event EventHandler<WindowProcEventArgs> OnWindowProc;


        public event EventHandler<MouseOverUrlChangedEventArgs> OnMouseoverUrlChange
        {
            add
            {
                if (m_mouseOverUrlChangedHandler == null)
                {
                    MBApi.wkeOnMouseOverUrlChanged(m_WebView, m_wkeMouseOverUrlChangedCallback, IntPtr.Zero);
                }
                m_mouseOverUrlChangedHandler += value;
            }
            remove
            {
                m_mouseOverUrlChangedHandler -= value;
                if (m_mouseOverUrlChangedHandler == null)
                {
                    MBApi.wkeOnMouseOverUrlChanged(m_WebView, null, IntPtr.Zero);
                }
            }
        }


        public event EventHandler<TitleChangeEventArgs> OnTitleChange 
        {
            add 
            {
                if (m_titleChangeHandler == null)
                {
                    MBApi.wkeOnTitleChanged(m_WebView, m_wkeTitleChangedCallback, IntPtr.Zero);
                }
                m_titleChangeHandler += value;
            }
            remove 
            {
                m_titleChangeHandler -= value;
                if (m_titleChangeHandler == null)
                {
                    MBApi.wkeOnTitleChanged(m_WebView, null, IntPtr.Zero);
                }
            }
        }


        public event EventHandler<UrlChangeEventArgs> OnURLChange 
        {
            add
            {
                if (m_urlChangeHandler == null)
                {
                    MBApi.wkeOnURLChanged2(m_WebView, m_wkeURLChangedCallback2, IntPtr.Zero);
                }
                m_urlChangeHandler += value;
            }
            remove
            {
                m_urlChangeHandler -= value;
                if (m_urlChangeHandler == null)
                {
                    MBApi.wkeOnURLChanged2(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<AlertBoxEventArgs> OnAlertBox 
        {
            add
            {
                if (m_alertBoxHandler == null)
                {
                    MBApi.wkeOnAlertBox(m_WebView, m_wkeAlertBoxCallback, IntPtr.Zero);
                }
                m_alertBoxHandler += value;
            }
            remove
            {
                m_alertBoxHandler -= value;
                if (m_alertBoxHandler == null)
                {
                    MBApi.wkeOnAlertBox(m_WebView, null, IntPtr.Zero);
                }
            }
        }
 
        public event EventHandler<ConfirmBoxEventArgs> OnConfirmBox
        {
            add
            {
                if (m_confirmBoxHandler == null)
                {
                    MBApi.wkeOnConfirmBox(m_WebView, m_wkeConfirmBoxCallback, IntPtr.Zero);
                }
                m_confirmBoxHandler += value;
            }
            remove
            {
                m_confirmBoxHandler -= value;
                if (m_confirmBoxHandler == null)
                {
                    MBApi.wkeOnConfirmBox(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<PromptBoxEventArgs> OnPromptBox 
        {
            add
            {
                if (m_promptBoxHandler == null)
                {
                    MBApi.wkeOnPromptBox(m_WebView, m_wkePromptBoxCallback, IntPtr.Zero);
                }
                m_promptBoxHandler += value;
            }
            remove
            {
                m_promptBoxHandler -= value;
                if (m_promptBoxHandler == null)
                {
                    MBApi.wkeOnPromptBox(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<NavigateEventArgs> OnNavigate 
        {
            add
            {
                if (m_navigateHandler == null)
                {
                    MBApi.wkeOnNavigation(m_WebView, m_wkeNavigationCallback, IntPtr.Zero);
                }
                m_navigateHandler += value;
            }
            remove
            {
                m_navigateHandler -= value;
                if (m_navigateHandler == null)
                {
                    MBApi.wkeOnNavigation(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<CreateViewEventArgs> OnCreateView {
            add
            {
                if (m_createViewHandler == null)
                {
                    MBApi.wkeOnCreateView(m_WebView, m_wkeCreateViewCallback, IntPtr.Zero);
                }
                m_createViewHandler += value;
            }
            remove
            {
                m_createViewHandler -= value;
                if (m_createViewHandler == null)
                {
                    MBApi.wkeOnCreateView(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<DocumentReadyEventArgs> OnDocumentReady 
        {
            add
            {
                if (m_documentReadyHandler == null)
                {
                    MBApi.wkeOnDocumentReady2(m_WebView, m_wkeDocumentReadyCallback, IntPtr.Zero);
                }
                m_documentReadyHandler += value;
            }
            remove
            {
                m_documentReadyHandler -= value;
                if (m_documentReadyHandler == null)
                {
                    MBApi.wkeOnDocumentReady2(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<LoadingFinishEventArgs> OnLoadingFinish 
        {
            add
            {
                if (m_loadingFinishHandler == null)
                {
                    MBApi.wkeOnLoadingFinish(m_WebView, m_wkeLoadingFinishCallback, IntPtr.Zero);
                }
                m_loadingFinishHandler += value;
            }
            remove
            {
                m_loadingFinishHandler -= value;
                if (m_loadingFinishHandler == null)
                {
                    MBApi.wkeOnLoadingFinish(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<DownloadEventArgs> OnDownload 
        {
            add
            {
                if (m_downloadHandler == null)
                {
                    MBApi.wkeOnDownload(m_WebView, m_wkeDownloadCallback, IntPtr.Zero);
                }
                m_downloadHandler += value;
            }
            remove
            {
                m_downloadHandler -= value;
                if (m_downloadHandler == null)
                {
                    MBApi.wkeOnDownload(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<ConsoleEventArgs> OnConsole 
        {
            add
            {
                if (m_consoleHandler == null)
                {
                    MBApi.wkeOnConsole(m_WebView, m_wkeConsoleCallback, IntPtr.Zero);
                }
                m_consoleHandler += value;
            }
            remove
            {
                m_consoleHandler -= value;
                if (m_consoleHandler == null)
                {
                    MBApi.wkeOnConsole(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<LoadUrlBeginEventArgs> OnLoadUrlBegin 
        {
            add
            {
                if (m_loadUrlBeginHandler == null)
                {
                    MBApi.wkeOnLoadUrlBegin(m_WebView, m_wkeLoadUrlBeginCallback, IntPtr.Zero);
                }
                m_loadUrlBeginHandler += value;
            }
            remove
            {
                m_loadUrlBeginHandler -= value;
                if (m_loadUrlBeginHandler == null)
                {
                    MBApi.wkeOnLoadUrlBegin(m_WebView, null, IntPtr.Zero);
                }
            }
        }
 
        public event EventHandler<LoadUrlEndEventArgs> OnLoadUrlEnd
        {
            add
            {
                if (m_loadUrlEndHandler == null)
                {
                    MBApi.wkeOnLoadUrlEnd(m_WebView, m_wkeLoadUrlEndCallback, IntPtr.Zero);
                }
                m_loadUrlEndHandler += value;
            }
            remove
            {
                m_loadUrlEndHandler -= value;
                if (m_loadUrlEndHandler == null)
                {
                    MBApi.wkeOnLoadUrlEnd(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<DidCreateScriptContextEventArgs> OnDidCreateScriptContext 
        {
            add
            {
                if (m_didCreateScriptContextHandler == null)
                {
                    MBApi.wkeOnDidCreateScriptContext(m_WebView, m_wkeDidCreateScriptContextCallback, IntPtr.Zero);
                }
                m_didCreateScriptContextHandler += value;
            }
            remove
            {
                m_didCreateScriptContextHandler -= value;
                if (m_didCreateScriptContextHandler == null)
                {
                    MBApi.wkeOnDidCreateScriptContext(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<WillReleaseScriptContextEventArgs> OnWillReleaseScriptContext 
        {
            add
            {
                if (m_willReleaseScriptContextHandler == null)
                {
                    MBApi.wkeOnWillReleaseScriptContext(m_WebView, m_wkeWillReleaseScriptContextCallback, IntPtr.Zero);
                }
                m_willReleaseScriptContextHandler += value;
            }
            remove
            {
                m_willReleaseScriptContextHandler -= value;
                if (m_willReleaseScriptContextHandler == null)
                {
                    MBApi.wkeOnWillReleaseScriptContext(m_WebView, null, IntPtr.Zero);
                }
            }
        }


        public event EventHandler<NetResponseEventArgs> OnNetResponse
        {
            add
            {
                if (m_netResponseHandler == null)
                {
                    MBApi.wkeNetOnResponse(m_WebView, m_wkeNetResponseCallback, IntPtr.Zero);
                }
                m_netResponseHandler += value;
            }
            remove
            {
                m_netResponseHandler -= value;
                if (m_netResponseHandler == null)
                {
                    MBApi.wkeNetOnResponse(m_WebView, null, IntPtr.Zero);
                }
            }
        }
  
        public event EventHandler<WillMediaLoadEventArgs> OnWillMediaLoad
        {
            add
            {
                if (m_willMediaLoadHandler == null)
                {
                    MBApi.wkeOnWillMediaLoad(m_WebView, m_wkeWillMediaLoadCallback, IntPtr.Zero);
                }
                m_willMediaLoadHandler += value;
            }
            remove
            {
                m_willMediaLoadHandler -= value;
                if (m_willMediaLoadHandler == null)
                {
                    MBApi.wkeOnWillMediaLoad(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<OtherLoadEventArgs> OnOtherLoad
        {
            add
            {
                if (m_OtherLoadHandler == null)
                {
                    MBApi.wkeOnOtherLoad(m_WebView, m_wkeOnOtherLoadCallback, IntPtr.Zero);
                }
                m_OtherLoadHandler += value;
            }
            remove
            {
                m_OtherLoadHandler -= value;
                if (m_OtherLoadHandler == null)
                {
                    MBApi.wkeOnOtherLoad(m_WebView, null, IntPtr.Zero);
                }
            }
        }

        private void SetEventCallBack()
        {
            m_wkeNetResponseCallback = new wkeNetResponseCallback((IntPtr WebView, IntPtr param, IntPtr url, IntPtr job) => {
                if (m_netResponseHandler != null)
                {
                    NetResponseEventArgs e = new NetResponseEventArgs(WebView, url, job);
                    m_netResponseHandler(this, e);
                    if (e.Cancel)
                        return 1;
                }
                return 0;
            });

            m_wkeTitleChangedCallback = new wkeTitleChangedCallback((IntPtr WebView, IntPtr param, IntPtr title) =>
            {
                if (m_titleChangeHandler != null)
                {
                    m_titleChangeHandler(this, new TitleChangeEventArgs(WebView, title));
                }

            });

            m_wkeMouseOverUrlChangedCallback = new wkeMouseOverUrlChangedCallback((IntPtr WebView, IntPtr param, IntPtr url) =>
            {
                if (m_titleChangeHandler != null)
                {
                    m_titleChangeHandler(this, new TitleChangeEventArgs(WebView, url));
                }

            });


            m_wkeURLChangedCallback2 = new wkeURLChangedCallback2((IntPtr WebView, IntPtr param, IntPtr frame, IntPtr url) =>
            {
                if (m_urlChangeHandler != null)
                {
                    m_urlChangeHandler(this, new UrlChangeEventArgs(WebView, url, frame));
                }

            });

            m_wkeAlertBoxCallback = new wkeAlertBoxCallback((IntPtr WebView, IntPtr param, IntPtr msg) =>
            {
                if (m_alertBoxHandler != null)
                {
                    m_alertBoxHandler(this, new AlertBoxEventArgs(WebView, msg));
                }
            });

            m_wkeConfirmBoxCallback = new wkeConfirmBoxCallback((IntPtr WebView, IntPtr param, IntPtr msg) =>
            {
                if (m_confirmBoxHandler != null)
                {
                    ConfirmBoxEventArgs e = new ConfirmBoxEventArgs(WebView, msg);
                    m_confirmBoxHandler(this, e);
                    return Convert.ToByte(e.Result);
                }
                return 0;
            });

            m_wkePromptBoxCallback = new wkePromptBoxCallback((IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult, IntPtr result) =>
            {
                if (m_promptBoxHandler != null)
                {
                    PromptBoxEventArgs e = new PromptBoxEventArgs(webView, msg, defaultResult, result);
                    m_promptBoxHandler(this, e);
                    return Convert.ToByte(e.Result);
                }
                return 0;
            });

            m_wkeNavigationCallback = new wkeNavigationCallback((IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url) =>
            {
                if (m_navigateHandler != null)
                {
                    NavigateEventArgs e = new NavigateEventArgs(webView, navigationType, url);
                    m_navigateHandler(this, e);
                    return (byte)(e.Cancel ? 0 : 1);
                }
                return 1;
            });

            m_wkeCreateViewCallback = new wkeCreateViewCallback((IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url, IntPtr windowFeatures) =>
            {
                if (m_createViewHandler != null)
                {
                    CreateViewEventArgs e = new CreateViewEventArgs(webView, navigationType, url, windowFeatures);
                    m_createViewHandler(this, e);
                    return e.NewWebViewHandle;
                }
                return webView;
            });


            m_wkeDocumentReadyCallback = new wkeDocumentReady2Callback((IntPtr webView, IntPtr param, IntPtr frame) =>
            {
                if (m_documentReadyHandler != null)
                {
                    m_documentReadyHandler(this, new DocumentReadyEventArgs(webView, frame));
                }
            });

            m_wkeLoadingFinishCallback = new wkeLoadingFinishCallback((IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason) =>
            {
                if (m_loadingFinishHandler != null)
                {
                    m_loadingFinishHandler(this, new LoadingFinishEventArgs(webView, url, result, failedReason));
                }
            });

            m_wkeDownloadCallback = new wkeDownloadCallback((IntPtr webView, IntPtr param, IntPtr url) =>
            {
                if (m_downloadHandler != null)
                {
                    DownloadEventArgs e = new DownloadEventArgs(webView, url);
                    m_downloadHandler(this, e);
                    return (byte)(e.Cancel ? 0 : 1);
                }
                return 1;
            });

            m_wkeConsoleCallback = new wkeConsoleCallback((IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace) =>
            {
                if (m_consoleHandler != null)
                {
                    m_consoleHandler(this, new ConsoleEventArgs(webView, level, message, sourceName, sourceLine, stackTrace));
                }
            });

            m_wkeLoadUrlBeginCallback = new wkeLoadUrlBeginCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr job) =>
            {
                if (m_loadUrlBeginHandler != null)
                {
                    LoadUrlBeginEventArgs e = new LoadUrlBeginEventArgs(webView, url, job);
                    m_loadUrlBeginHandler(this, e);
                    return (byte)(e.Cancel ? 1 : 0);
                }
                return 0;
            });

            m_wkeLoadUrlEndCallback = new wkeLoadUrlEndCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr job, IntPtr buf, int len) =>
            {
                if (m_loadUrlEndHandler != null)
                {
                    LoadUrlEndEventArgs e = new LoadUrlEndEventArgs(webView, url, job, buf, len);
                    m_loadUrlEndHandler(this, e);
                }
            });

            m_wkeDidCreateScriptContextCallback = new wkeDidCreateScriptContextCallback((IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int extensionGroup, int worldId) =>
            {
                if (m_didCreateScriptContextHandler != null)
                {
                    DidCreateScriptContextEventArgs e = new DidCreateScriptContextEventArgs(webView, frame, context, extensionGroup, worldId);
                    m_didCreateScriptContextHandler(this, e);
                }
            });

            m_wkeWillReleaseScriptContextCallback = new wkeWillReleaseScriptContextCallback((IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int worldId) =>
            {
                if (m_willReleaseScriptContextHandler != null)
                {
                    WillReleaseScriptContextEventArgs e = new WillReleaseScriptContextEventArgs(webView, frame, context, worldId);
                    m_willReleaseScriptContextHandler(this, e);
                }
            });

            m_wkeWillMediaLoadCallback = new wkeWillMediaLoadCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr info) => 
            {
                if (m_willMediaLoadHandler != null)
                {
                    WillMediaLoadEventArgs e = new WillMediaLoadEventArgs(webView, url, info);
                    m_willMediaLoadHandler(this, e);
                }
            });

            m_wkeOnOtherLoadCallback = new wkeOnOtherLoadCallback((IntPtr webView, IntPtr param, wkeOtherLoadType type, IntPtr info) =>
            {
                if (m_OtherLoadHandler != null)
                {
                    OtherLoadEventArgs e = new OtherLoadEventArgs(webView, type, info);
                    m_OtherLoadHandler(this, e);
                }
            });

        }


        #endregion

        
        public WebView()
        {
            m_wkePaintUpdatedCallback = new wkePaintUpdatedCallback(wkeOnPaintUpdated);
            m_WndProcCallback = new WndProcCallback(OnWndProc);
            this.SetEventCallBack();
        }

        public WebView(IWin32Window window, bool isTransparent = false)
        {
            m_wkePaintUpdatedCallback = new wkePaintUpdatedCallback(wkeOnPaintUpdated);
            m_WndProcCallback = new WndProcCallback(OnWndProc);
            this.SetEventCallBack();
            this.Bind(window, isTransparent);
        }

        #region IDisposable
        
        public void Dispose()
        {
            if (m_WebView != IntPtr.Zero)
            {
                if (m_OldProc != IntPtr.Zero)
                {
                    Help.SetWindowLong(m_hWnd, (int)WinConst.GWL_WNDPROC, m_OldProc.ToInt32());
                    m_OldProc = IntPtr.Zero;
                }
                MBApi.wkeSetHandle(m_WebView, IntPtr.Zero);
                MBApi.wkeDestroyWebView(m_WebView);
                m_WebView = IntPtr.Zero;
                m_hWnd = IntPtr.Zero;
            }
        }
        #endregion

        protected void wkeOnPaintUpdated(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            IntPtr hWnd = param;
            if ((int)WinConst.WS_EX_LAYERED == ((int)WinConst.WS_EX_LAYERED & Help.GetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE)))
            {
                RECT rectDest = new RECT();
                Help.GetWindowRect(m_hWnd, ref rectDest);
                Help.OffsetRect(ref rectDest, -rectDest.Left, -rectDest.Top);

                SIZE sizeDest = new SIZE(rectDest.Right - rectDest.Left, rectDest.Bottom - rectDest.Top);
                //POINT pointDest = new POINT(); // { rectDest.left, rectDest.top };
                POINT pointSource = new POINT();

                BITMAP bmp = new BITMAP();
                IntPtr hBmp = Help.GetCurrentObject(hdc, (int)WinConst.OBJ_BITMAP);
                Help.GetObject(hBmp, Marshal.SizeOf(typeof(BITMAP)), ref bmp);

                sizeDest.cx = bmp.bmWidth;
                sizeDest.cy = bmp.bmHeight;

                IntPtr hdcScreen = Help.GetDC(hWnd);

                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = (byte)WinConst.AC_SRC_OVER;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = (byte)WinConst.AC_SRC_ALPHA;
                int callOk = Help.UpdateLayeredWindow(m_hWnd, hdcScreen, IntPtr.Zero, ref sizeDest, hdc, ref pointSource, 0, ref blend, (int)WinConst.ULW_ALPHA);
                if (callOk == 0)
                {
                    IntPtr hdcMemory = Help.CreateCompatibleDC(hdcScreen);
                    IntPtr hbmpMemory = Help.CreateCompatibleBitmap(hdcScreen, sizeDest.cx, sizeDest.cy);
                    IntPtr hbmpOld = Help.SelectObject(hdcMemory, hbmpMemory);

                    Help.BitBlt(hdcMemory, 0, 0, sizeDest.cx, sizeDest.cy, hdc, 0, 0, (int)WinConst.SRCCOPY | (int)WinConst.CAPTUREBLT);

                    Help.BitBlt(hdc, 0, 0, sizeDest.cx, sizeDest.cy, hdcMemory, 0, 0, (int)WinConst.SRCCOPY | (int)WinConst.CAPTUREBLT); //!

                    callOk = Help.UpdateLayeredWindow(m_hWnd, hdcScreen, IntPtr.Zero, ref sizeDest, hdcMemory, ref pointSource, 0, ref blend, (int)WinConst.ULW_ALPHA);

                    Help.SelectObject(hdcMemory, hbmpOld);
                    Help.DeleteObject(hbmpMemory);
                    Help.DeleteDC(hdcMemory);
                }

                Help.ReleaseDC(m_hWnd, hdcScreen);
            }
            else
            {
                RECT rc = new RECT(x, y, x + cx, y + cy);
                Help.InvalidateRect(m_hWnd, ref rc, true);
            }

        }
        protected IntPtr OnWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (OnWindowProc != null)
            {
                WindowProcEventArgs e = new WindowProcEventArgs(hWnd, (int)msg, wParam, lParam);
                OnWindowProc(this, e);
                if (e.bHand)
                    return e.Result;
            }

            switch (msg)
            {
                case (uint)WinConst.WM_PAINT:
                    {
                        if ((int)WinConst.WS_EX_LAYERED != ((int)WinConst.WS_EX_LAYERED & (int)Help.GetWindowLong(hWnd, (int)WinConst.GWL_EXSTYLE)))
                        {
                            MBApi.wkeRepaintIfNeeded(m_WebView);

                            PAINTSTRUCT ps = new PAINTSTRUCT();
                            IntPtr hdc = Help.BeginPaint(hWnd, ref ps);

                            RECT rcClip = ps.rcPaint;

                            RECT rcClient = new RECT();
                            Help.GetClientRect(hWnd, ref rcClient);

                            RECT rcInvalid = rcClient;
                            if (rcClip.Right != rcClip.Left && rcClip.Bottom != rcClip.Top)
                                Help.IntersectRect(ref rcInvalid, ref rcClip, ref rcClient);

                            int srcX = rcInvalid.Left - rcClient.Left;
                            int srcY = rcInvalid.Top - rcClient.Top;
                            int destX = rcInvalid.Left;
                            int destY = rcInvalid.Top;
                            int width = rcInvalid.Right - rcInvalid.Left;
                            int height = rcInvalid.Bottom - rcInvalid.Top;

                            if (0 != width && 0 != height)
                                Help.BitBlt(hdc, destX, destY, width, height, MBApi.wkeGetViewDC(m_WebView), srcX, srcY, (int)WinConst.SRCCOPY);

                            Help.EndPaint(hWnd, ref ps);
                            return IntPtr.Zero;
                        }
                    }
                    break;
                case (uint)WinConst.WM_ERASEBKGND:
                    return new IntPtr(1);
                case (uint)WinConst.WM_SIZE:
                    {
                        int width = lParam.ToInt32() & 65535;
                        int height = lParam.ToInt32() >> 16;
                        MBApi.wkeResize(m_WebView, width, height);
                        MBApi.wkeRepaintIfNeeded(m_WebView);
                    }
                    break;
                case (uint)WinConst.WM_KEYDOWN:
                    {
                        int virtualKeyCode = wParam.ToInt32();
                        uint flags = 0;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_REPEAT) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_EXTENDED) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;

                        if (MBApi.wkeFireKeyDownEvent(m_WebView, virtualKeyCode, flags, false)!=0)
                            return IntPtr.Zero;
                    }
                    break;
                case (uint)WinConst.WM_KEYUP:
                    {
                        int virtualKeyCode = wParam.ToInt32();
                        uint flags = 0;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_REPEAT) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_EXTENDED) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;

                        if (MBApi.wkeFireKeyUpEvent(m_WebView, virtualKeyCode, flags, false) != 0)
                            return IntPtr.Zero;
                    }
                    break;
                case (uint)WinConst.WM_CHAR:
                    {
                        int charCode = wParam.ToInt32();
                        uint flags = 0;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_REPEAT) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        if (((lParam.ToInt32() >> 16) & (int)WinConst.KF_EXTENDED) != 0)
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;

                        if (MBApi.wkeFireKeyPressEvent(m_WebView, charCode, flags, false) != 0)
                            return IntPtr.Zero;
                    }
                    break;
                case (uint)WinConst.WM_LBUTTONDOWN:
                case (uint)WinConst.WM_MBUTTONDOWN:
                case (uint)WinConst.WM_RBUTTONDOWN:
                case (uint)WinConst.WM_LBUTTONDBLCLK:
                case (uint)WinConst.WM_MBUTTONDBLCLK:
                case (uint)WinConst.WM_RBUTTONDBLCLK:
                case (uint)WinConst.WM_LBUTTONUP:
                case (uint)WinConst.WM_MBUTTONUP:
                case (uint)WinConst.WM_RBUTTONUP:
                case (uint)WinConst.WM_MOUSEMOVE:
                    {
                        if (msg == (uint)WinConst.WM_LBUTTONDOWN || msg == (uint)WinConst.WM_MBUTTONDOWN || msg == (uint)WinConst.WM_RBUTTONDOWN)
                        {
                            if (Help.GetFocus() != hWnd)
                                Help.SetFocus(hWnd);
                            Help.SetCapture(hWnd);
                        }
                        else if (msg == (uint)WinConst.WM_LBUTTONUP || msg == (uint)WinConst.WM_MBUTTONUP || msg == (uint)WinConst.WM_RBUTTONUP)
                        {
                            Help.ReleaseCapture();
                        }

                        int x = Help.LOWORD(lParam);
                        int y = Help.HIWORD(lParam);

                        uint flags = 0;

                        if ((wParam.ToInt32() & (int)WinConst.MK_CONTROL) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        if ((wParam.ToInt32() & (int)WinConst.MK_SHIFT) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;

                        if ((wParam.ToInt32() & (int)WinConst.MK_LBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_MBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_RBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;

                        if (MBApi.wkeFireMouseEvent(m_WebView, msg, x, y, flags) != 0)
                        {
                            //(msg);
                            return IntPtr.Zero;
                        }
                    }
                    break;
                case (uint)WinConst.WM_CONTEXTMENU:
                    {
                        POINT pt;
                        pt.x = Help.LOWORD(lParam);
                        pt.y = Help.HIWORD(lParam);

                        if (pt.x != -1 && pt.y != -1)
                            Help.ScreenToClient(hWnd, ref pt);

                        uint flags = 0;

                        if ((wParam.ToInt32() & (int)WinConst.MK_CONTROL) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        if ((wParam.ToInt32() & (int)WinConst.MK_SHIFT) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;

                        if ((wParam.ToInt32() & (int)WinConst.MK_LBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_MBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_RBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;

                        if (MBApi.wkeFireContextMenuEvent(m_WebView, pt.x, pt.y, flags) != 0)
                            return IntPtr.Zero;

                    }
                    break;
                case (uint)WinConst.WM_MOUSEWHEEL:
                    {
                        POINT pt;
                        pt.x = Help.LOWORD(lParam);
                        pt.y = Help.HIWORD(lParam);
                        Help.ScreenToClient(hWnd, ref pt);

                        int delta = Help.HIWORD(wParam);

                        uint flags = 0;

                        if ((wParam.ToInt32() & (int)WinConst.MK_CONTROL) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        if ((wParam.ToInt32() & (int)WinConst.MK_SHIFT) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;

                        if ((wParam.ToInt32() & (int)WinConst.MK_LBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_MBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        if ((wParam.ToInt32() & (int)WinConst.MK_RBUTTON) != 0)
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;

                        if (MBApi.wkeFireMouseWheelEvent(m_WebView, pt.x, pt.y, delta, flags) != 0)
                            return IntPtr.Zero;
                        break;
                    }
                case (uint)WinConst.WM_SETFOCUS:
                    MBApi.wkeSetFocus(m_WebView);
                    return IntPtr.Zero;

                case (uint)WinConst.WM_KILLFOCUS:
                    MBApi.wkeKillFocus(m_WebView);
                    return IntPtr.Zero;

                case (uint)WinConst.WM_SETCURSOR:
                    if (MBApi.wkeFireWindowsMessage(m_WebView, hWnd, (uint)WinConst.WM_SETCURSOR, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0)
                        return IntPtr.Zero;
                    break;

                case (uint)WinConst.WM_IME_STARTCOMPOSITION:
                    {
                        wkeRect caret = MBApi.wkeGetCaretRect(m_WebView);

                        COMPOSITIONFORM COMPOSITIONFORM = new COMPOSITIONFORM();
                        COMPOSITIONFORM.dwStyle = (int)WinConst.CFS_POINT | (int)WinConst.CFS_FORCE_POSITION;
                        COMPOSITIONFORM.ptCurrentPos.x = caret.x;
                        COMPOSITIONFORM.ptCurrentPos.y = caret.y;

                        IntPtr hIMC = Help.ImmGetContext(hWnd);
                        Help.ImmSetCompositionWindow(hIMC, ref COMPOSITIONFORM);
                        Help.ImmReleaseContext(hWnd, hIMC);
                    }
                    return IntPtr.Zero;
                //case (uint)WinConst.WM_NCDESTROY:
                    //IntPtr ret = Help.CallWindowProc(m_OldProc, hWnd, msg, wParam, lParam);
                    //this.Dispose();
                    //return Help.DefWindowProc(hWnd, msg, wParam, lParam);
                case (uint)WinConst.WM_INPUTLANGCHANGE:
                    return Help.DefWindowProc(hWnd, msg, wParam, lParam);
                   
            }
            return Help.CallWindowProc(m_OldProc, hWnd, msg, wParam, lParam);
        }

        


        #region
      
        public static void wkeInitialize()
        {
            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitialize();
            }
        }
 

        public static void wkeInitialize(wkeSettings settings)
        {
            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitializeEx(settings);
            }
        }

        public static void wkeFinalize()
        {
            if (MBApi.wkeIsInitialize() != 0)
            {
                MBApi.wkeFinalize();
            }
        }

        public static uint Version
        {
            get { return MBApi.wkeGetVersion(); }
        }
   
        public static string VersionString
        {
            get
            {
                IntPtr utf8 = MBApi.wkeGetVersionString();
                if (utf8 != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(utf8);
                }
                return null;
            }
        }

        public static void SetProxy(wkeProxy proxy)
        {
            MBApi.wkeSetProxy(ref proxy);
        }

        public static bool FrameIsMainFrame(IntPtr WebFrame)
        {
            return MBApi.wkeIsMainFrame(WebFrame) != 0;
        }
  
        public static bool FrameIsRemoteFrame(IntPtr WebFrame)
        {
            return MBApi.wkeIsWebRemoteFrame(WebFrame) != 0;
        }
  
        public static IntPtr FrameGetMainWorldScriptContext(IntPtr WebFrame)
        {
            IntPtr v8ContextPtr = IntPtr.Zero;
            MBApi.wkeWebFrameGetMainWorldScriptContext(WebFrame,ref v8ContextPtr);
            return v8ContextPtr;
        }

 
        public static IntPtr GetBlinkMainThreadIsolate()
        {
            return MBApi.wkeGetBlinkMainThreadIsolate();
        }


        public static void NetSetMIMEType(IntPtr job, string MIMEType)
        {
            MBApi.wkeNetSetMIMEType(job, MIMEType);
        }
 
        public static string NetGetMIMEType(IntPtr job)
        {
            IntPtr mime = MBApi.wkeCreateStringW(null, 0);
            if (mime == IntPtr.Zero)
            {
                return string.Empty;
            }
            MBApi.wkeNetGetMIMEType(job, mime);
            IntPtr pStr = MBApi.wkeGetStringW(mime);
            string mimeType = Help.PtrToStringUTF8(pStr);
            MBApi.wkeDeleteString(mime);
            return mimeType;

        }

        public static void NetSetHTTPHeaderField(IntPtr job, string key, string value, bool response)
        {
            MBApi.wkeNetSetHTTPHeaderField(job, key, value, response);
        }
 
        public static void NetSetURL(IntPtr job, string URL)
        {
            MBApi.wkeNetSetURL(job, URL);
        }

        public static void NetSetData(IntPtr job, byte[] data)
        {
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        public static void NetSetData(IntPtr job, string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        public static void NetSetData(IntPtr job, System.Drawing.Image png)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                png.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                data = ms.GetBuffer();
            }
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        public static void NetSetData(IntPtr job, System.Drawing.Image img, System.Drawing.Imaging.ImageFormat fmt)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, fmt);
                data = ms.GetBuffer();
            }
            MBApi.wkeNetSetData(job, data, data.Length);
        }


        public static void NetHookRequest(IntPtr job)
        {
            MBApi.wkeNetHookRequest(job);
        }

        public static int NetGetFavicon(IntPtr WebView, wkeNetResponseCallback Callback, IntPtr param)
        {
            return MBApi.wkeNetGetFavicon(WebView, Callback, param);
        }
        

        public static void VisitAllCookie(wkeCookieVisitor visitor, IntPtr userData)
        {
            MBApi.wkeVisitAllCookie(userData, visitor);
        }


        public static void PerformCookieCommand(wkeCookieCommand command)
        {
            MBApi.wkePerformCookieCommand(command);
        }


        public static void SetFileSystem(FileSystem fs)
        {
            if (fs != null)
            {
                MBApi.wkeSetFileSystem(fs.m_fileOpen, fs.m_fileClose, fs.m_fileSize, fs.m_fileRead, fs.m_fileSeek);
            }
            else 
            {
                MBApi.wkeSetFileSystem(null, null, null, null, null);
            }
        }



        #endregion

        #region 

        
        public bool Bind(IWin32Window window, bool isTransparent = false)
        {
            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitialize();
            }

            if (m_hWnd == window.Handle)
            {
                return true;
            }

            if (m_WebView == IntPtr.Zero)
            {
                m_WebView = MBApi.wkeCreateWebView();
                if (m_WebView == IntPtr.Zero)
                {
                    return false;
                }
            }
            m_hWnd = window.Handle;

            MBApi.wkeSetHandle(m_WebView, m_hWnd);
            MBApi.wkeOnPaintUpdated(m_WebView, m_wkePaintUpdatedCallback, m_hWnd);

            if (isTransparent)
            {
                MBApi.wkeSetTransparent(m_WebView, true);
                int exStyle = Help.GetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE);
                Help.SetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE, exStyle | (int)WinConst.WS_EX_LAYERED);
            }
            else
            {
                MBApi.wkeSetTransparent(m_WebView, false);
            }
            m_OldProc = Help.GetWindowLongIntPtr(m_hWnd, (int)WinConst.GWL_WNDPROC);
            if (m_OldProc != Marshal.GetFunctionPointerForDelegate(m_WndProcCallback))
            {
                m_OldProc = Help.SetWindowLongDelegate(m_hWnd, (int)WinConst.GWL_WNDPROC, m_WndProcCallback);
            }
            RECT rc = new RECT();
            Help.GetClientRect(m_hWnd, ref rc);
            MBApi.wkeResize(m_WebView, rc.Right - rc.Left, rc.Bottom - rc.Top);
            
            return true;
        }


        public void Load(string URL)
        {
            MBApi.wkeLoadW(m_WebView, URL);
        }


        public void LoadURL(string URL)
        {
            MBApi.wkeLoadURLW(m_WebView, URL);
        }

        public void LoadFile(string FileName)
        {
            MBApi.wkeLoadFileW(m_WebView, FileName);
        }
 
        public void LoadHTML(string Html)
        {
            MBApi.wkeLoadHTMLW(m_WebView, Html);
        }

        public void PostURL(string URL, byte[] PostData)
        {
            MBApi.wkePostURLW(m_WebView, URL, PostData, PostData.Length);
        }

        public void LoadHtmlWithBaseUrl(string Html, string BaseURL)
        {
            MBApi.wkeLoadHtmlWithBaseUrl(m_WebView, Encoding.UTF8.GetBytes(Html), Encoding.UTF8.GetBytes(BaseURL));
        }

 
        public void StopLoading()
        {
            MBApi.wkeStopLoading(m_WebView);
        }

        public void Reload()
        {
            MBApi.wkeReload(m_WebView);
        }


        public string GetURL()
        {
            IntPtr pUrl = MBApi.wkeGetURL(m_WebView);
            if (pUrl != IntPtr.Zero)
                return Help.PtrToStringUTF8(pUrl);
            return string.Empty;
        }

        public string GetFrameURL(IntPtr FrameId)
        {
            IntPtr pUrl = MBApi.wkeGetFrameUrl(m_WebView, FrameId);
            if (pUrl != IntPtr.Zero)
                return Help.PtrToStringUTF8(pUrl);
            return string.Empty;
        }


        public void GC(int delayMs)
        {
            MBApi.wkeGC(m_WebView, delayMs);
        }

        public void SetViewProxy(wkeProxy proxy)
        {
            MBApi.wkeSetViewProxy(m_WebView, ref proxy);
        }
 
        public void Sleep()
        {
            MBApi.wkeSleep(m_WebView);
        }
 
        public void Wake()
        {
            MBApi.wkeWake(m_WebView);
        }

        public void SetUserAgent(string UserAgent)
        {
            MBApi.wkeSetUserAgentW(m_WebView, UserAgent);
        }

        public string GetUserAgent()
        {
            IntPtr pstr = MBApi.wkeGetUserAgent(m_WebView);
            if (pstr != IntPtr.Zero)
            {
                return Help.PtrToStringUTF8(pstr);
            }
            return string.Empty;
        }


        public bool GoBack()
        {
            return MBApi.wkeGoBack(m_WebView) != 0;
        }
 
        public bool GoForward()
        {
            return MBApi.wkeGoForward(m_WebView) != 0;
        }

        public void EditorSelectAll()
        {
            MBApi.wkeEditorSelectAll(m_WebView);
        }

        public void EditorUnSelect()
        {
            MBApi.wkeEditorUnSelect(m_WebView);
        }

        public void EditorCopy()
        {
            MBApi.wkeEditorCopy(m_WebView);
        }

        public void EditorCut()
        {
            MBApi.wkeEditorCut(m_WebView);
        }

        public void EditorPaste()
        {
            MBApi.wkeEditorPaste(m_WebView);
        }

        public void EditorDelete()
        {
            MBApi.wkeEditorDelete(m_WebView);
        }
 
        public void EditorUndo()
        {
            MBApi.wkeEditorUndo(m_WebView);
        }

        public void EditorRedo()
        {
            MBApi.wkeEditorRedo(m_WebView);
        }

        public string GetCookie()
        {
            IntPtr pStr = MBApi.wkeGetCookieW(m_WebView);
            if (pStr != IntPtr.Zero)
            {
                return Marshal.PtrToStringUni(pStr);
            }
            return string.Empty;
        }
 
        public void SetCookie(string Url, string Cookie)
        {
            byte[] url = Encoding.UTF8.GetBytes(Url);
            byte[] cookie = Encoding.UTF8.GetBytes(Cookie);
            MBApi.wkeSetCookie(m_WebView, url, cookie);
        }


        public void SetCookieJarPath(string Path)
        {
            MBApi.wkeSetCookieJarPath(m_WebView, Path);
        }

        public void SetCookieJarFullPath(string FileName)
        {
            MBApi.wkeSetCookieJarFullPath(m_WebView, FileName);
        }
 
        public void SetLocalStorageFullPath(string Path)
        {
            MBApi.wkeSetLocalStorageFullPath(m_WebView, Path);
        }

        public void AddPluginDirectory(string Path)
        {
            MBApi.wkeAddPluginDirectory(m_WebView, Path);
        }


        public void SetFocus()
        {
            MBApi.wkeSetFocus(m_WebView);
        }

        public void KillFocus()
        {
            MBApi.wkeKillFocus(m_WebView);
        }
 
        public JsValue RunJS(string JavaScript)
        {
            return MBApi.wkeRunJSW(m_WebView, JavaScript);
        }


        public JsValue RunJsByFrame(IntPtr FrameId, string JavaScript, bool IsInClosure = false)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(JavaScript);
            return MBApi.wkeRunJsByFrame(m_WebView, FrameId, utf8, IsInClosure);
        }

        public IntPtr GlobalExec()
        {
            return MBApi.wkeGlobalExec(m_WebView);
        }

        public IntPtr GetGlobalExecByFrame(IntPtr FrameId)
        {
            return MBApi.wkeGetGlobalExecByFrame(m_WebView, FrameId);
        }


        public void SetEditable(bool editable)
        {
            MBApi.wkeSetEditable(m_WebView, editable);
        }

        public void SetCspCheckEnable(bool enable)
        {
            MBApi.wkeSetCspCheckEnable(m_WebView, enable);
        }

        public void SetNetInterface(string NetInterface)
        {
            MBApi.wkeSetViewNetInterface(m_WebView, NetInterface);
        }

        public Bitmap PrintToBitmap()
        {
            if (m_WebView == IntPtr.Zero)
                return null;
            MBApi.wkeRunJSW(m_WebView, @"document.body.style.overflow='hidden'");
            MBApi.wkeRepaintIfNeeded(m_WebView);
            int w = MBApi.wkeGetContentWidth(m_WebView);
            int h = MBApi.wkeGetContentHeight(m_WebView);

            int oldwidth = MBApi.wkeGetWidth(m_WebView);
            int oldheight = MBApi.wkeGetHeight(m_WebView);

            MBApi.wkeResize(m_WebView, w, h);

            Bitmap bmp = new Bitmap(w, h);
            Rectangle rc = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(rc, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            MBApi.wkePaint(m_WebView, data.Scan0, 0);
            bmp.UnlockBits(data);

            MBApi.wkeResize(m_WebView, oldwidth, oldheight);
            MBApi.wkeRunJSW(m_WebView, @"document.body.style.overflow='visible'");

            return bmp;
        }

        public void SetDebugConfig(string debugString, string param)
        {
            MBApi.wkeSetDebugConfig(m_WebView, debugString, param);
        }


        public void SetDeviceParameter(string device, string paramStr, int paramInt = 0, float paramFloat = 0)
        {
            MBApi.wkeSetDeviceParameter(m_WebView, device, paramStr, paramInt, paramFloat);
        }

        public void ShowDevtools(string Path, wkeOnShowDevtoolsCallback Callback, IntPtr Param)
        {
            MBApi.wkeShowDevtools(m_WebView, Path, Callback, Param);
        }


        public void DeleteWillSendRequestInfo(IntPtr WillSendRequestInfoPtr)
        {
            MBApi.wkeDeleteWillSendRequestInfo(m_WebView, WillSendRequestInfoPtr);
        }

        public void InsertCSSByFrame(IntPtr FrameId, string cssText)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(cssText);
            MBApi.wkeInsertCSSByFrame(m_WebView, FrameId, utf8);
        }



        #endregion

        #region 
     
        public IntPtr Handle
        {
            get { return m_WebView; }
        }


        public IntPtr HostHandle
        {
            get { return m_hWnd; }
        }


        public string Name
        {
            get
            {
                IntPtr pName = MBApi.wkeGetName(m_WebView);
                if (pName != IntPtr.Zero)
                    return Marshal.PtrToStringAnsi(pName);
                return string.Empty;
            }
            set
            {
                MBApi.wkeSetName(m_WebView, value);
            }
        }

        public bool IsWake
        {
            get { return MBApi.wkeIsAwake(m_WebView) != 0; }
        }
 
        public bool IsLoading
        {
            get { return MBApi.wkeIsLoading(m_WebView) != 0; }
        }

        public bool IsLoadingSucceeded
        {
            get { return MBApi.wkeIsLoadingSucceeded(m_WebView) != 0; }
        }

        public bool IsLoadingFailed
        {
            get { return MBApi.wkeIsLoadingFailed(m_WebView) != 0; }
        }

        public bool IsLoadingCompleted
        {
            get { return MBApi.wkeIsLoadingCompleted(m_WebView) != 0; }
        }

        public bool IsDocumentReady
        {
            get { return MBApi.wkeIsDocumentReady(m_WebView) != 0; }
        }

        public string Title
        {
            get 
            {
                IntPtr pTitle = MBApi.wkeGetTitleW(m_WebView);
                if (pTitle != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(pTitle);
                }
                return string.Empty;
            }
        }

        public int Width
        {
            get { return MBApi.wkeGetWidth(m_WebView); }
        }

        public int Height
        {
            get { return MBApi.wkeGetHeight(m_WebView); }
        }

        public int ContentWidth
        {
            get { return MBApi.wkeGetContentWidth(m_WebView); }
        }

        public int ContentHeight
        {
            get { return MBApi.wkeGetContentHeight(m_WebView); }
        }

        public bool CanGoBack
        {
            get { return MBApi.wkeCanGoBack(m_WebView) != 0; }
        }

        public bool CanGoForward
        {
            get { return MBApi.wkeCanGoForward(m_WebView) != 0; }
        }


        public bool CookieEnabled
        {
            get { return MBApi.wkeIsCookieEnabled(m_WebView) != 0; }
            set { MBApi.wkeSetCookieEnabled(m_WebView, value); }
        }
 
        public float MediaVolume
        {
            get { return MBApi.wkeGetMediaVolume(m_WebView); }
            set { MBApi.wkeSetMediaVolume(m_WebView, value); }
        }

        public float ZoomFactor
        {
            get { return MBApi.wkeGetZoomFactor(m_WebView); }
            set { MBApi.wkeSetZoomFactor(m_WebView, value); }
        }

        public IntPtr MainFrame
        {
            get { return MBApi.wkeWebFrameGetMainFrame(m_WebView); }
        }

        public wkeCursorInfo CursorInfoType
        {
            get { return MBApi.wkeGetCursorInfoType(m_WebView); }
        }


        public bool MemoryCacheEnable
        {
            set
            {
                MBApi.wkeSetMemoryCacheEnable(m_WebView, value);
            }
        }


        public bool NavigationToNewWindowEnable
        {
            set { MBApi.wkeSetNavigationToNewWindowEnable(m_WebView, value); }
        }

        public bool TouchEnable
        {
            set { MBApi.wkeSetTouchEnabled(m_WebView, value); }
        }

  
        public bool NpapiPluginsEnabled
        {
            set { MBApi.wkeSetNpapiPluginsEnabled(m_WebView, value); }
        }

        public bool HeadlessEnabled
        {
            set { MBApi.wkeSetHeadlessEnabled(m_WebView, value); }
        }


        public bool DragEnable
        {
            set { MBApi.wkeSetDragEnable(m_WebView, value); }
        }

        public bool DragDropEnable
        {
            set { MBApi.wkeSetDragDropEnable(m_WebView, value); }
        }

        public int ResourceGc
        {
            set { MBApi.wkeSetResourceGc(m_WebView, value); }
        }


        public bool IsProcessingUserGesture
        {
            get { return MBApi.wkeIsProcessingUserGesture(m_WebView) != 0; }
        }

        #endregion

    }

    #region
    public class MiniblinkEventArgs : EventArgs
    {
        private IntPtr m_webView;

        public MiniblinkEventArgs(IntPtr webView)
        {
            m_webView = webView;
        }

        public IntPtr Handle
        {
            get { return m_webView; }
        }
    }


    public class MouseOverUrlChangedEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public MouseOverUrlChangedEventArgs(IntPtr webView, IntPtr url)
            : base(webView)
        {
            m_url = url;
        }

        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    IntPtr pTitle = MBApi.wkeGetStringW(m_url);
                    if (pTitle != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pTitle);
                }
                return string.Empty;
            }
        }
    }

    public class TitleChangeEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_title;

        public TitleChangeEventArgs(IntPtr webView, IntPtr title)
            : base(webView)
        {
            m_title = title;
        }

        public string Title
        {
            get
            {
                if (m_title != IntPtr.Zero)
                {
                    IntPtr pTitle = MBApi.wkeGetStringW(m_title);
                    if (pTitle != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pTitle);
                }
                return string.Empty;
            }
        }
    }

    public class UrlChangeEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_frame;

        public UrlChangeEventArgs(IntPtr webView, IntPtr url, IntPtr webFrame)
            : base(webView)
        {
            m_url = url;
            m_frame = webFrame;
        }

        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    IntPtr pUrl = MBApi.wkeGetStringW(m_url);
                    if (pUrl != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pUrl);
                }
                return string.Empty;
            }
        }

        public IntPtr WebFrame
        {
            get { return m_frame; }
        }

    }

    public class AlertBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;

        public AlertBoxEventArgs(IntPtr webView, IntPtr msg)
            : base(webView)
        {
            m_msg = msg;
        }

        public string Msg
        {
            get
            {
                if (m_msg != IntPtr.Zero)
                {
                    IntPtr pMsg = MBApi.wkeGetStringW(m_msg);
                    if (pMsg != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pMsg);
                }
                return string.Empty;
            }
        }
    }

    public class ConfirmBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;
        private bool m_result;

        public ConfirmBoxEventArgs(IntPtr webView, IntPtr msg)
            : base(webView)
        {
            m_msg = msg;
        }

        public string Msg
        {
            get
            {
                if (m_msg != IntPtr.Zero)
                {
                    IntPtr pMsg = MBApi.wkeGetStringW(m_msg);
                    if (pMsg != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pMsg);
                }
                return string.Empty;
            }
        }

        public bool Result
        {
            get { return m_result; }
            set { m_result = value; }
        }

    }

    public class PromptBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;
        private bool m_result;
        private IntPtr m_defaultStr;
        private IntPtr m_resultStr;

        public PromptBoxEventArgs(IntPtr webView, IntPtr msg, IntPtr defaultResult, IntPtr result)
            : base(webView)
        {
            m_msg = msg;
            m_defaultStr = defaultResult;
            m_resultStr = result;
        }

        public string Msg
        {
            get
            {
                if (m_msg != IntPtr.Zero)
                {
                    IntPtr pMsg = MBApi.wkeGetStringW(m_msg);
                    if (pMsg != IntPtr.Zero)
                        return Marshal.PtrToStringUni(pMsg);
                }
                return string.Empty;
            }
        }

        public bool Result
        {
            get { return m_result; }
            set { m_result = value; }
        }


        public string DefaultResultString
        {
            get 
            {
                if (m_defaultStr != IntPtr.Zero)
                {
                    IntPtr pStr = MBApi.wkeGetStringW(m_defaultStr);
                    return Marshal.PtrToStringUni(pStr);
                }
                return string.Empty;
            }
        }

        public string ResultString
        {
            set 
            {
                if (m_resultStr != IntPtr.Zero)
                {
                    MBApi.wkeSetStringW(m_resultStr, value, value.Length);
                }
            }
        }

    }

    public class NavigateEventArgs : MiniblinkEventArgs
    {
        private wkeNavigationType m_type;
        private IntPtr m_url;
        private bool m_result;

        public NavigateEventArgs(IntPtr webView, wkeNavigationType navigationType, IntPtr url)
            : base(webView)
        {
            m_type = navigationType;
            m_url = url;
        }

        public wkeNavigationType NavigationType
        {
            get { return m_type; }
        }
 
        public string URL
        {
            get 
            {
                if (m_url != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_url));
                }
                return string.Empty;
            }
        }
 
        public bool Cancel
        {
            get { return m_result; }
            set { m_result = value; }
        }
    }
  
    public class CreateViewEventArgs : MiniblinkEventArgs
    {
        private wkeNavigationType m_type;
        private IntPtr m_url;
        private IntPtr m_windowFeatures;
        private IntPtr m_result;

        public CreateViewEventArgs(IntPtr webView, wkeNavigationType navigationType, IntPtr url, IntPtr windowFeatures)
            : base(webView)
        {
            m_type = navigationType;
            m_url = url;
            m_windowFeatures = windowFeatures;
            m_result = webView;
        }

        public wkeNavigationType NavigationType
        {
            get { return m_type; }
        }

        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_url));
                }
                return string.Empty;
            }
        }

        public wkeWindowFeatures WindowFeatures
        {
            get 
            {
                if (m_windowFeatures != IntPtr.Zero)
                {
                    return (wkeWindowFeatures)Marshal.PtrToStructure(m_windowFeatures, typeof(wkeWindowFeatures));
                }
                else
                {
                    return new wkeWindowFeatures();
                }
            }
        }

   
        public IntPtr NewWebViewHandle
        {
            get { return m_result; }
            set { m_result = value; }
        }

    }
 
    public class DocumentReadyEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_frame;

        public DocumentReadyEventArgs(IntPtr webView, IntPtr frame)
            : base(webView)
        {
            m_frame = frame;
        }
  
        public IntPtr Frame
        {
            get { return m_frame; }
        }
    }
   
    public class LoadingFinishEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private wkeLoadingResult m_loadingResult;
        private IntPtr m_failedReason;

        public LoadingFinishEventArgs(IntPtr webView, IntPtr url, wkeLoadingResult result, IntPtr failedReason)
            : base(webView)
        {
            m_url = url;
            m_loadingResult = result;
            m_failedReason = failedReason;
        }
    
        public string URL
        {
            get 
            {
                if (m_url != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_url));
                }
                return string.Empty;
            }
        }
    
        public wkeLoadingResult LoadingResult
        {
            get { return m_loadingResult; }
        }
  
        public string FailedReason
        {
            get 
            {
                if (m_failedReason != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_failedReason));
                }
                return string.Empty;
            }
        }
    }
 
    public class DownloadEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private bool m_cancel;

        public DownloadEventArgs(IntPtr webView, IntPtr url)
            : base(webView)
        {
            m_url = url;
        }
 
        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    return Help.PtrToStringUTF8(m_url);
                }
                return string.Empty;
            }
        }
 
        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }
    }
 
    public class ConsoleEventArgs : MiniblinkEventArgs
    {
        private wkeConsoleLevel m_level;
        private IntPtr m_message;
        private IntPtr m_sourceName;
        private uint m_sourceLine;
        private IntPtr m_stackTrace;

        public ConsoleEventArgs(IntPtr webView, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace)
            : base(webView)
        {
            m_level = level;
            m_message = message;
            m_sourceName = sourceName;
            m_sourceLine = sourceLine;
            m_stackTrace = stackTrace;
        }
  
        public wkeConsoleLevel Level
        {
            get { return m_level; }
        }
     
        public string Message
        {
            get 
            {
                if (m_message != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_message));
                }
                return string.Empty;
            }
        }
  
        public string SourceName
        {
            get
            {
                if (m_sourceName != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_sourceName));
                }
                return string.Empty;
            }
        }
  
        public uint SourceLine
        {
            get { return m_sourceLine; }
        }
  
        public string StackTrace
        {
            get
            {
                if (m_stackTrace != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(MBApi.wkeGetStringW(m_stackTrace));
                }
                return string.Empty;
            }
        }

    }
 
    public class LoadUrlBeginEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_job;
        private bool m_cancel;

        public LoadUrlBeginEventArgs(IntPtr webView, IntPtr url, IntPtr job)
            : base(webView)
        {
            m_url = url;
            m_job = job;
        }
  
        public string URL
        {
            get 
            {
                if (m_url != IntPtr.Zero)
                {
                    return Help.PtrToStringUTF8(m_url);
                }
                return string.Empty;
            }
        }

        public IntPtr Job
        {
            get { return m_job; }
        }
 
        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }
    }
   
    public class LoadUrlEndEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_job;
        private IntPtr m_buf;
        private int m_len;

        public LoadUrlEndEventArgs(IntPtr webView, IntPtr url, IntPtr job, IntPtr buf, int len)
            : base(webView)
        {
            m_url = url;
            m_job = job;
            m_buf = buf;
            m_len = len;
        }
 
        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    return Help.PtrToStringUTF8(m_url);
                }
                return string.Empty;
            }
        }
 
        public IntPtr Job
        {
            get { return m_job; }
        }
  
        public byte[] Data
        {
            get
            {
                if (m_buf != IntPtr.Zero)
                {
                    byte[] data = new byte[m_len];
                    Marshal.Copy(m_buf, data, 0, m_len);
                    return data;
                }
                return null;
            }
        }
    }
 
    public class DidCreateScriptContextEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_frame;
        private IntPtr m_context;
        private int m_extensionGroup;
        private int m_worldId;

        public DidCreateScriptContextEventArgs(IntPtr webView, IntPtr frame, IntPtr context, int extensionGroup, int worldId)
            : base(webView)
        {
            m_frame = frame;
            m_context = context;
            m_extensionGroup = extensionGroup;
            m_worldId = worldId;
        }
  
        public IntPtr Frame
        {
            get { return m_frame; }
        }
   
        public IntPtr Context
        {
            get { return m_context; }
        }

        public int ExtensionGroup
        {
            get { return m_extensionGroup; }
        }

        public int WorldId
        {
            get { return m_worldId; }
        }

    }
  
    public class WillReleaseScriptContextEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_frame;
        private IntPtr m_context;
        private int m_worldId;

        public WillReleaseScriptContextEventArgs(IntPtr webView, IntPtr frame, IntPtr context, int worldId)
            : base(webView)
        {
            m_frame = frame;
            m_context = context;
            m_worldId = worldId;
        }
 
        public IntPtr Frame
        {
            get { return m_frame; }
        }
 
        public IntPtr Context
        {
            get { return m_context; }
        }

        public int WorldId
        {
            get { return m_worldId; }
        }
    }

    public class NetResponseEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_job;
        private bool m_cancel;

        public NetResponseEventArgs(IntPtr webView, IntPtr url, IntPtr job)
            : base(webView)
        {
            m_url = url;
            m_job = job;
        }

        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    return Help.PtrToStringUTF8(m_url);
                }
                return string.Empty;
            }
        }

        public IntPtr Job
        {
            get { return m_job; }
        }

        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }
    }
 
    public class WillMediaLoadEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private wkeMediaLoadInfo m_info;

        public WillMediaLoadEventArgs(IntPtr webView, IntPtr url, IntPtr info)
            : base(webView)
        {
            m_url = url;
            m_info = (wkeMediaLoadInfo)Marshal.PtrToStructure(info, typeof(wkeMediaLoadInfo));
        }

        public string URL
        {
            get
            {
                if (m_url != IntPtr.Zero)
                {
                    return Help.PtrToStringUTF8(m_url);
                }
                return string.Empty;
            }
        }

        public wkeMediaLoadInfo Info
        {
            get { return m_info; }
        }
    }
 
    public class OtherLoadEventArgs : MiniblinkEventArgs
    {
        private wkeOtherLoadType m_type;
        private wkeTempCallbackInfo m_info;

        public OtherLoadEventArgs(IntPtr webView, wkeOtherLoadType type, IntPtr info)
            : base(webView)
        {
            m_type = type;
            m_info = (wkeTempCallbackInfo)Marshal.PtrToStructure(info, typeof(wkeTempCallbackInfo));
        }
 
        public wkeOtherLoadType LoadType
        {
            get { return m_type; }
        }

        public int Size
        {
            get { return m_info.size; }
        }
 
        public IntPtr Frame
        {
            get { return m_info.frame; }
        }

        public wkeWillSendRequestInfo WillSendRequestInfo
        {
            get 
            {
                wkeWillSendRequestInfo srInfo = new wkeWillSendRequestInfo();
                if (m_info.willSendRequestInfo != IntPtr.Zero)
                {
                    IntPtr ptr = IntPtr.Zero;
                    IntPtr strPtr = IntPtr.Zero;
                    srInfo.isHolded = Marshal.ReadInt32(m_info.willSendRequestInfo) != 0;

                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 4);
                    if (ptr != IntPtr.Zero)
                    {
                        strPtr = MBApi.wkeGetStringW(ptr);
                        if (strPtr != IntPtr.Zero)
                        {
                            srInfo.url = Marshal.PtrToStringUni(strPtr);
                        }
                    }
                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 8);
                    if (ptr != IntPtr.Zero)
                    {
                        strPtr = MBApi.wkeGetStringW(ptr);
                        if (strPtr != IntPtr.Zero)
                        {
                            srInfo.newUrl = Marshal.PtrToStringUni(strPtr);
                        }
                    }
                    srInfo.resourceType = (wkeResourceType)Marshal.ReadInt32(m_info.willSendRequestInfo, 12);
                    srInfo.httpResponseCode = Marshal.ReadInt32(m_info.willSendRequestInfo, 16);
                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 20);
                    if (ptr != IntPtr.Zero)
                    {
                        strPtr = MBApi.wkeGetStringW(ptr);
                        if (strPtr != IntPtr.Zero)
                        {
                            srInfo.method = Marshal.PtrToStringUni(strPtr);
                        }
                    }
                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 24);
                    if (ptr != IntPtr.Zero)
                    {
                        strPtr = MBApi.wkeGetStringW(ptr);
                        if (strPtr != IntPtr.Zero)
                        {
                            srInfo.referrer = Marshal.PtrToStringUni(strPtr);
                        }
                    }
                    srInfo.headers = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 28);
                }
                return srInfo;
            }
        }

        public IntPtr WillSendRequestInfoPtr
        {
            get { return m_info.willSendRequestInfo; }
        }
    }


    public class WindowProcEventArgs : EventArgs
    {
        private IntPtr m_hWnd;
        private int m_msg;
        private IntPtr m_wParam;
        private IntPtr m_lParam;
        private IntPtr m_result;
        private bool m_bHand;

        public WindowProcEventArgs(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            m_hWnd = hWnd;
            m_msg = msg;
            m_wParam = wParam;
            m_lParam = lParam;
        }
 
        public IntPtr Handle
        {
            get { return m_hWnd; }
        }
 
        public int Msg
        {
            get { return m_msg; }
        }
 
        public IntPtr wParam
        {
            get { return m_wParam; }
        }
  
        public IntPtr lParam
        {
            get { return m_lParam; }
        }
 
        public IntPtr Result
        {
            get { return m_result; }
            set { m_result = value; }
        }
 
        public bool bHand
        {
            get { return m_bHand; }
            set { m_bHand = value; }
        }
    }


    #endregion

    internal delegate IntPtr OnWindowProcEventHandler(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);


}
