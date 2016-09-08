using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace Avalonia.Controls
{
    /// <summary>
    /// 	A control to host a native window.
    /// </summary>
    public abstract class NativeWindowHost : Control, IKeyboardInputSink, IDisposable
    {    	
    	protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			
			System.Diagnostics.Debug.WriteLine("Keyup.");
		}
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			
			System.Diagnostics.Debug.WriteLine("Keydown.");
		}
		

		
		protected override void OnGotFocus(GotFocusEventArgs e)
		{
			
			// Call The TabInto method when the control gots focus to 
			// let the native window handle getting focus.
			Contract.Requires<ArgumentNullException>(e != null);
			
			NavigationMethod direction;
							
//			if (e.InputModifiers == InputModifiers.Shift)
//				direction = NavigationMethod.Previous;
//			else
//				direction = NavigationMethod.Next;
//				
//			var handled = ((IKeyboardInputSink)this).TabInto(direction);
//			if (handled)
//				e.Handled = handled;
			
			base.OnGotFocus(e);
		}
        
		protected override void OnLostFocus(RoutedEventArgs e)
		{						
			base.OnLostFocus(e);
		}
		
		
		
		
		
		
        /// <summary>
        /// 	The static <see cref="NativeWindowHost"/> constructor.
        /// </summary>
        static NativeWindowHost()
        {
        	// Set default value of FocusableProperty to true.
            FocusableProperty.OverrideMetadata(typeof(NativeWindowHost),
                                               new StyledPropertyMetadata<bool>(true));
        	
        	// Register IgnoreNativeWindowRectProperty.
            NativeWindowHost.IgnoreNativeWindowRectProperty = 
            	AvaloniaProperty.Register<NativeWindowHost, bool>(
            		"IgnoreNativeWindowRect");
            
            // Property IgnoreNativeWindowRectProperty affects measurement
            Avalonia.Layout.Layoutable.AffectsMeasure(
            	new AvaloniaProperty[]
            	{
            		NativeWindowHost.IgnoreNativeWindowRectProperty
            	}
            );
        }

        /// <summary>
        /// 	Creates a new instance of <see cref="NativeWindowHost"/>.
        /// </summary>
        protected NativeWindowHost()
        {
            Initialize();
        }

        public static readonly StyledProperty<bool> IgnoreNativeWindowRectProperty;

        // Desired size
        private Size _desiredSize = new Size(
        	double.PositiveInfinity, double.PositiveInfinity);
        
        // Desired position        
        private Point _desiredPosition = new Point(0.0, 0.0);
        
        // Indicates the window is built        
        private bool _isBuilt = false;
        
        // The list with the disposables
        private List<IDisposable> _disposables = new List<IDisposable>();

        /// <summary>
        /// 	Set to <b>True</b> to ignore the native window rectangle.
        /// </summary>
        public bool IgnoreNativeWindowRect
        {
            get { return GetValue<bool>(NativeWindowHost.IgnoreNativeWindowRectProperty); }
            set { SetValue<bool>(NativeWindowHost.IgnoreNativeWindowRectProperty, value); }
        }

        /// <summary>
        /// 	Occurs, when the window was build.
        /// </summary>
        public event EventHandler Built;

        /// <summary>
        /// 	Gets a value indicating whether 
        /// 	the native window has been disposed of.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// 	The platform window handle of the hosted window.
        /// </summary>
        public IPlatformHandle Handle { get; private set; }
        
        /// <summary>
        /// 	Initialize host.
        /// </summary>
        private void Initialize()
        {
            this.Built += (sender, e) =>
            {
            	var root = VisualRoot as TopLevel;
            	if (root == null)
                    throw new NotSupportedException("The visual root is not a toplevel.");

                // Track change in bounds.
                var disposable = new BoundsTracker()
                    .Track(this)
                    .Subscribe(UpdateLayout);
                _disposables.Add(disposable);
            };
        }
        
        /// <summary>
        /// 	Updates the layout.
        /// </summary>
        private void UpdateLayout(TransformedBounds? bounds)
        {
        	if(bounds == null)
        		return;
        	if (IsDisposed)
                return;
        	
            if (!bounds.Value.Bounds.IsEmpty)
            {
                var rect = CalculateWindowRect(bounds.Value.Bounds);
                UpdateWindowPosition(rect);
                
                var trans = CalculateWindowTransformation(bounds.Value.Transform);
                UpdateWindowTrans(trans);
            }
        }
        
        /// <summary>
        /// 	Calculates the window rect.
        /// </summary>
        /// <param name="rect">
        /// 	The bounds relative to the top level window.
        /// </param>
        /// <returns>
        /// 	The determined window rect.
        /// </returns>
        private Rect CalculateWindowRect(Rect rect)
        {
        	var position = _desiredPosition + rect.TopLeft + new Point(0.0, 0.0);
            var s = _desiredSize;
            
            var size = new Size(
                DesiredSize.Width - position.X + 0.0,
                DesiredSize.Height - position.Y);
            
            return new Rect(position, size);
        }
        
        /// <summary>
        /// 	Updates the natives window position. Derived classes override
        /// 	this method to set the position of the native window.
        /// </summary>
        /// <param name="rectangle">
        /// 	The rectangle to the new window has to be positioned.
        /// </param>
        /// <seealso cref="GetWindowStartRectangle"/>
        /// <example>
        /// 	_window.Resize(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        /// </example>
        protected abstract void UpdateWindowPosition(Rect rectangle);
        
        /// <summary>
        /// 	Calculates the window transformation matrix.
        /// </summary>
        /// <param name="transformation">
        /// 	The bounds transformation matrix.
        /// </param>
        /// <returns>
        /// 	The determined window transformation matrix.
        /// </returns>
		private Matrix CalculateWindowTransformation(Matrix transformation)
		{
			return transformation;
		}
		
        /// <summary>
        /// 	Updates the transformation of the window.
        /// </summary>
        /// <param name="transformationMatrix">
        /// 	The transformation matrix to apply.
        /// </param>
        ///		_window.Transform(
        /// 		transformationMatrix.M11,
        /// 		transformationMatrix.M12,
        /// 		transformationMatrix.M21,
        /// 		transformationMatrix.M22,
        /// 		transformationMatrix.M31,
        /// 		transformationMatrix.M32
        /// 	);
        protected virtual void UpdateWindowTrans(Matrix transformationMatrix) { }
        
        /// <summary> 
        ///     Called when the control is added to a visual tree. 
        /// </summary> 
        /// <param name="e">
        ///     The event args.
        /// </param>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            
            var root = e.Root as TopLevel;

            if (root == null)
                throw new NotSupportedException("The visual root is not a toplevel.");
            
            BuildWindow(root);
        }

        /// <summary> 
        /// 	Called when the control is removed from a visual tree.
        /// </summary>
        /// <param name="e">
        /// 	The event args.
        /// </param>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            Dispose();
        }
        
        /// <summary>
        /// 	Return the desired size of the native window.
        /// </summary>
        /// <param name="availableSize">
        /// 	The available size.
        /// </param>
        /// <returns>
        /// 	The desired size.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {        	
            var width = 0.0;
            var height = 0.0;

            if (this.Handle != null && this.Handle.Handle != IntPtr.Zero)
            {
                width = Math.Min(_desiredSize.Width, availableSize.Width);
                height = Math.Min(_desiredSize.Height, availableSize.Height);

                if (double.IsPositiveInfinity(width))
                    width = MinWidth;

                if (double.IsPositiveInfinity(height))
                    height = MinHeight;
            }

            return new Size(width, height);
        }
        
        /// <summary>
        /// 	Creates the window to be hosted. Derived classes override this
        /// 	method to build the window being hosted.
        /// </summary>
        /// <param name="parent">
        /// 	The platform handle of the parent window.
        /// </param>
        /// <returns>
        /// 	The platform handle of the created window.
        /// </returns>
        /// <seealso cref="Destroy"/>
        /// <example>
        /// 	// Create a platform specific window with the handle of the root window.
        /// 	_window = WindowFactory.CreatePlatformWindow(parent.Handle);
        /// 	
        /// 	// Return a new platform handle with the created window handle.
        /// 	return new PlatformHandle(_window.Handle, "ChildWindow");
        /// </example>
        protected abstract IPlatformHandle Build(IPlatformHandle parent);

        /// <summary>
        /// 	Destroys the hosted window. Derived classes should override
        /// 	this method to destroy the hosted window.
        /// </summary>
        /// <param name="child">
        /// 	The platform handle of the child.
        /// </param>
        /// <seealso cref="Build"/>
        /// <example>
        /// 	_window.Dispose();
        /// </example>
        protected abstract void Destroy(IPlatformHandle child);

        /// <summary>
        /// 	Returns the rectangle of the native window. Derived classes override
        /// 	this to set the window rectangle relative to the control.
        /// </summary>
        /// <returns>
        /// 	The rectangle of the native window.
        /// </returns>
        /// <seealso cref="UpdateWindowPosition"/>
        /// <remarks>
        /// 	This will only be determined once.
        /// </remarks>
        /// <example>
        /// 	return new Rect(_window.X, _window.Y, _window.Width, _window.Height);
        /// </example>
        protected abstract Rect GetWindowStartRectangle();
        
        bool IKeyboardInputSink.TabInto(NavigationMethod navigationMethod) 
        { return TabIntoCore(navigationMethod); }
        
        /// <summary>
        ///     Sets focus on either the first tab stop or the last tab stop of the sink.
        /// </summary>
		/// <param name="navigationMethod">
		/// 	Specifies whether focus should be set to the first or the last tab stop.
		/// </param>
		/// <returns>
		/// 	<b>True</b> if the focus has been set as requested. 
		/// 	<b>False</b>, if there are no tab stops.
		/// </returns>
        /// <remarks>
        /// 	You can get <see cref="FocusNavigationDirection.Previous"/> for
        /// 	the last native control (means Shift+Tab was pressed), or 
        /// 	<see cref="FocusNavigationDirection.Next"/> for the first 
        /// 	native control (means Tab was pressed).
        /// </remarks>
        protected virtual bool TabIntoCore(NavigationMethod navigationMethod) { return false; }
        
        void IKeyboardInputSink.TabOut(NavigationMethod navigationMethod)
		{ TabOutCore(navigationMethod); }
        
        /// <summary>
		/// 	Sets focus on either the previous tab stop or the next tab stop of the sink.
		/// </summary>
		/// <param name="navigationMethod">
		/// 	Specifies whether the focus should be set no the previous or the next
		/// 	tab stop relative to this one.
		/// </param>
        protected virtual void TabOutCore(NavigationMethod navigationMethod) { }
                
        /// <summary>
        /// 	Builds the window.
        /// </summary>
        /// <param name="parent">
        /// 	The platform handle of the parent window.
        /// </param>
        private void BuildWindow(TopLevel parent)
        {
            if (_isBuilt)
                throw new InvalidOperationException("The window was already built.");

            if (parent.PlatformImpl.Handle == null || parent.PlatformImpl.Handle.Handle == IntPtr.Zero)
                throw new NotSupportedException("The handle of the parent window cannot be null.");

            this.Handle = Build(parent.PlatformImpl.Handle);

            if (this.Handle == null || this.Handle.Handle == IntPtr.Zero)
                throw new InvalidOperationException("The Build method created an invalid window handle.");

            // Get rectangle of native window
            if (!IgnoreNativeWindowRect)
            {
                var rect = GetWindowStartRectangle();                
                var upperLeft = rect.TopLeft;
                var lowerRight = rect.BottomRight;
                // TODO convert units from pixel to desired unit
                // TODO handle right to left
                
                // set desired rect
                var p = rect.Position;
                _desiredSize = new Size(lowerRight.X - upperLeft.X, lowerRight.Y - upperLeft.Y); // Currently rect.Size
                _desiredPosition = new Point(rect.X, rect.Y); // Currently rect.Position
            }

            this.OnBuilded(EventArgs.Empty);
        }
        
        /// <summary>
        ///     Gets called when the window was build.
        /// </summary>
        /// <param name="e">
        ///     The event data.
        /// </param>
        protected void OnBuilded(EventArgs e)
        {
            _isBuilt = true;

            var del = Built;
            if (del != null)
                del(this, e);
        }

        /// <summary>
        /// 	Destroys the window.
        /// </summary>
        private void DestroyWindow()
        {
            if (this.Handle == null || this.Handle.Handle == IntPtr.Zero)
                return;

            var temp = Handle;
            Handle = new PlatformHandle(IntPtr.Zero, "null handle");

            Destroy(temp);
        }
        
		// TODO Dispose() in NativeWindowHost does not get called!!!  Only in finalizer the object gets distroyed.
        /// <summary>
        /// 	Performs application-defined tasks associated with freeing, 
        /// 	releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        	// Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 	Frees, release unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// 	If disposing equals true, the method has been called directly 
        /// 	or indirectly by a user's code. 
        /// 	Managed and unmanaged resources can be disposed.
        /// 	If disposing equals false, the method has been called by the
        /// 	runtime from inside the finalizer and you should not reference
        /// 	other objects. Only unmanaged resources can be disposed.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                _disposables.ForEach(x => x.Dispose());
            }
            // Free any unmanaged objects here.
            DestroyWindow();

            IsDisposed = true;
        }

        /// <summary>
        /// 	The finalizer of the class.
        /// </summary>
        ~NativeWindowHost()
        {        	
            Dispose(false);
        }
    }
}
