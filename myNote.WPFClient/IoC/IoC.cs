using myNote.WPFClient.View.Pages;
using myNote.WPFClient.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.IoC
{
    /// <summary>
    /// The IoC container for our application
    /// </summary>
    public static class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="WindowViewModel"/>
        /// </summary>
        public static WindowViewModel WindowViewModel => IoC.Get<WindowViewModel>();
        /// <summary>
        /// A shortcut to access the <see cref="MainPageViewModel"/>
        /// </summary>
        public static MainPageViewModel MainViewModel => IoC.Get<MainPageViewModel>();
        /// <summary>
        /// Link of api resources
        /// </summary>
        public const string ConnectionString = "http://mynoteapi.azurewebsites.net/api/";

        #endregion

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as your application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void SetupWindow()
        {
            // Bind window view model
            BindWindowViewModel();
        }

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called after login
        /// </summary>
        public static void SetupMainPage()
        {
            // Bind main page view model
            BindMainPageViewModel();
        }

        /// <summary>
        /// Binds all singleton window view model
        /// </summary>
        private static void BindWindowViewModel()
        {
            // Bind to a single instance of window view model
            Kernel.Bind<WindowViewModel>().ToConstant(new WindowViewModel());
        }
        /// <summary>
        /// Binds all singleton main page view model
        /// </summary>
        private static void BindMainPageViewModel()
        {
            //Bind to a single instance of main view model
            Kernel.Bind<MainPageViewModel>().ToConstant(new MainPageViewModel());
        }
        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
