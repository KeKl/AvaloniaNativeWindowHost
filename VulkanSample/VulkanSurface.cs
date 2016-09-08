using System;
using System.Linq;
using Vulkan;
using Vulkan.Windows;

namespace VulkanSample
{
	/// <summary>
	/// Description of VulkanSurface.
	/// </summary>
	public class VulkanSurface : Win32.Win32AvaloniaWindow
	{
		public VulkanSurface(IntPtr parent)
			: base(parent)
		{
			_instance = CreateInstance();
			InitializeVulkan();
		}
		
		private bool _initialized;
		private Instance _instance;
		private Device _device;
		private Queue _queue;
		private SwapchainKhr _swapchain;
		private Semaphore _semaphore;
		private Fence[] _fences;
		private CommandBuffer[] _commandBuffers;
		
		private Instance CreateInstance()
		{
			var app = new ApplicationInfo();
			app.ApiVersion = Vulkan.Version.Make(1, 0, 0);
			app.ApplicationName = "VulkanSample";
			app.ApplicationVersion = Vulkan.Version.Make(1, 0, 0);
			app.EngineName = "Engine0";
			app.EngineVersion = Vulkan.Version.Make(1, 0, 0);
			
			var info = new InstanceCreateInfo();
			info.ApplicationInfo = app;
			info.EnabledExtensionNames = new [] { "VK_KHR_surface", "VK_KHR_win32_surface" };
			
			return new Instance(info);
		}
		
		private SurfaceFormatKhr SelectFormat (PhysicalDevice physicalDevice, SurfaceKhr surface) 
		{
			foreach(var f in physicalDevice.GetSurfaceFormatsKHR(surface))
                if(f.Format == Format.R8g8b8a8Unorm || f.Format == Format.B8g8r8a8Unorm)
                    return f;
            throw new Exception("didn't find the R8g8b8a8Unorm format");
		}
		
		private SwapchainKhr CreateSwapchain(
			SurfaceKhr surface, SurfaceCapabilitiesKhr surfaceCapabilities, SurfaceFormatKhr surfaceFormat)
		{
			var swapchainInfo = new SwapchainCreateInfoKhr 
			{
				Surface = surface,
				MinImageCount = surfaceCapabilities.MinImageCount,
				ImageFormat = surfaceFormat.Format,
				ImageColorSpace = surfaceFormat.ColorSpace,
				ImageExtent = surfaceCapabilities.CurrentExtent,
				ImageUsage = ImageUsageFlags.ColorAttachment,
				PreTransform = SurfaceTransformFlagsKhr.Identity,
				ImageArrayLayers = 1,
				ImageSharingMode = SharingMode.Exclusive,
				QueueFamilyIndices = new uint [] { 0 },
				PresentMode = PresentModeKhr.Fifo,
				CompositeAlpha = CompositeAlphaFlagsKhr.Inherit
			};
			return _device.CreateSwapchainKHR(swapchainInfo);
		}
		
		private Framebuffer [] CreateFramebuffers(
			Image[] images, SurfaceFormatKhr surfaceFormat, SurfaceCapabilitiesKhr surfaceCapabilities, RenderPass renderPass)
		{
			var displayViews = new ImageView[images.Length];
			for (int i = 0; i < images.Length; i++) 
			{
				var viewCreateInfo = new ImageViewCreateInfo 
				{
					Image = images [i],
					ViewType = ImageViewType.View2D,
					Format = surfaceFormat.Format,
					Components = new ComponentMapping {
						R = ComponentSwizzle.R,
						G = ComponentSwizzle.G,
						B = ComponentSwizzle.B,
						A = ComponentSwizzle.A
					},
					SubresourceRange = new ImageSubresourceRange 
					{
						AspectMask = ImageAspectFlags.Color,
						LevelCount = 1,
						LayerCount = 1
					}
				};
				displayViews[i] = _device.CreateImageView(viewCreateInfo);
			}
			
			var framebuffers = new Framebuffer[images.Length];
			for (int i = 0; i < images.Length; i++) 
			{
				var frameBufferCreateInfo = new FramebufferCreateInfo 
				{
					Layers = 1,
					RenderPass = renderPass,
					Attachments = new [] { displayViews [i] },
					Width = surfaceCapabilities.CurrentExtent.Width,
					Height = surfaceCapabilities.CurrentExtent.Height
				};
				framebuffers[i] = _device.CreateFramebuffer(frameBufferCreateInfo);
			}
			return framebuffers;
		}
		
		private CommandBuffer[] CreateCommandBuffers(
			Image[] images, Framebuffer[] framebuffers, RenderPass renderPass, SurfaceCapabilitiesKhr surfaceCapabilities)
		{
			var createPoolInfo = new CommandPoolCreateInfo { Flags = CommandPoolCreateFlags.ResetCommandBuffer };
			var commandPool = _device.CreateCommandPool(createPoolInfo);
			var commandBufferAllocateInfo = new CommandBufferAllocateInfo 
			{
				Level = CommandBufferLevel.Primary,
				CommandPool = commandPool,
				CommandBufferCount = (uint)images.Length
			};
			var buffers = _device.AllocateCommandBuffers(commandBufferAllocateInfo);
			for (int i = 0; i < images.Length; i++) 
			{
				var commandBufferBeginInfo = new CommandBufferBeginInfo();
				buffers [i].Begin(commandBufferBeginInfo);
				var renderPassBeginInfo = new RenderPassBeginInfo 
				{
					Framebuffer = framebuffers[i],
					RenderPass = renderPass,
					ClearValues = new [] { new ClearValue { Color = new ClearColorValue (new [] { 0.9f, 0.7f, 0.0f, 1.0f }) } },
					RenderArea = new Rect2D { Extent = surfaceCapabilities.CurrentExtent }
				};
				buffers[i].CmdBeginRenderPass(renderPassBeginInfo, SubpassContents.Inline);
				buffers[i].CmdEndRenderPass();
				buffers[i].End ();
			}
			return buffers;
		}
		
		private RenderPass CreateRenderPass(SurfaceFormatKhr surfaceFormat)
		{
			var attDesc = new AttachmentDescription 
			{
				Format = surfaceFormat.Format,
				Samples = SampleCountFlags.Count1,
				LoadOp = AttachmentLoadOp.Clear,
				StoreOp = AttachmentStoreOp.Store,
				StencilLoadOp = AttachmentLoadOp.DontCare,
				StencilStoreOp = AttachmentStoreOp.DontCare,
				InitialLayout = ImageLayout.ColorAttachmentOptimal,
				FinalLayout = ImageLayout.ColorAttachmentOptimal
			};
			var attRef = new AttachmentReference { Layout = ImageLayout.ColorAttachmentOptimal };
			var subpassDesc = new SubpassDescription 
			{
				PipelineBindPoint = PipelineBindPoint.Graphics,
				ColorAttachments = new [] { attRef }
			};
			var renderPassCreateInfo = new RenderPassCreateInfo 
			{
				Attachments = new [] { attDesc },
				Subpasses = new [] { subpassDesc }
			};
			return _device.CreateRenderPass(renderPassCreateInfo);
		}
		
		private void InitializeVulkan()
		{
			var devices = _instance.EnumeratePhysicalDevices();
			var surface = _instance.CreateWin32SurfaceKHR(
				new Win32SurfaceCreateInfoKhr
				{
					Hinstance = Instance,
					Hwnd = Handle
				});
			var queueInfo = new DeviceQueueCreateInfo { QueuePriorities = new [] { 1.0f } };
			var deviceInfo = new DeviceCreateInfo 
			{
				EnabledExtensionNames = new [] { "VK_KHR_swapchain" },
				QueueCreateInfos = new [] { queueInfo }
			};
			var physicalDevice = devices [0]; 
			_device = physicalDevice.CreateDevice(deviceInfo);
			_queue = _device.GetQueue(0, 0);
			
			var surfaceCapabilities = physicalDevice.GetSurfaceCapabilitiesKHR(surface);
			var surfaceFormat = SelectFormat(physicalDevice, surface);
			_swapchain = CreateSwapchain(surface, surfaceCapabilities, surfaceFormat);
			var images = _device.GetSwapchainImagesKHR(_swapchain);
			var renderPass = CreateRenderPass(surfaceFormat);
			var framebuffers = CreateFramebuffers(images, surfaceFormat, surfaceCapabilities, renderPass);
			_commandBuffers = CreateCommandBuffers(images, framebuffers, renderPass, surfaceCapabilities);
			var fenceInfo = new FenceCreateInfo();
			_fences = new [] { _device.CreateFence(fenceInfo) };
			var semaphoreInfo = new SemaphoreCreateInfo();
			_semaphore = _device.CreateSemaphore(semaphoreInfo);
			
			_initialized = true;
		}
		
		protected override void Draw()
		{
			if (_initialized) 
				DrawFrame();
		}
		
		private void DrawFrame () 
		{
			uint nextIndex = _device.AcquireNextImageKHR
				(_swapchain, ulong.MaxValue, _semaphore, _fences [0]);
			_device.ResetFences(_fences);
			var submitInfo = new SubmitInfo 
			{
				WaitSemaphores = new [] { _semaphore },
				CommandBuffers = new [] { _commandBuffers [nextIndex] }
			};
			_queue.Submit(new [] { submitInfo }, _fences [0]);
			_device.WaitForFences(_fences, true, 100000000);
			var presentInfo = new PresentInfoKhr 
			{
				Swapchains = new [] { _swapchain },
				ImageIndices = new [] { nextIndex }
			};
			_queue.PresentKHR (presentInfo);
		}

		private void CleanUp()
		{
			_instance.Destroy();
			_device.Destroy();
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// free managed resources
			}
			
			base.Dispose(disposing);
			CleanUp();
		}
	}
}