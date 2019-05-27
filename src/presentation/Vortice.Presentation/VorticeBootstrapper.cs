// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Microsoft.VisualStudio.Composition;

namespace Vortice.Presentation
{
    // @see: https://github.com/dbuksbaum/Caliburn.Micro.Autofac/blob/master/src/Caliburn.Micro.Autofac.WPF/AutofacBootstrapper.cs

    public class VorticeBootstrapper : BootstrapperBase
    {
        protected ExportProvider ExportProvider { get; set; }

        public VorticeBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var discovery = PartDiscovery.Combine(
                new AttributedPartDiscovery(Resolver.DefaultInstance), // "NuGet MEF" attributes (Microsoft.Composition)
                new AttributedPartDiscoveryV1(Resolver.DefaultInstance)); // ".NET MEF" attributes (System.ComponentModel.Composition)

            // Build up a catalog of MEF parts
            var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
                .AddParts(discovery.CreatePartsAsync(Assembly.GetExecutingAssembly()).Result)
                .WithCompositionService(); // Makes an ICompositionService export available to MEF parts to import

            // Assemble the parts into a valid graph.
            var config = CompositionConfiguration.Create(catalog);

            // Prepare an ExportProvider factory based on this graph.
            var epf = config.CreateExportProviderFactory();

            // Create an export provider, which represents a unique container of values.
            // You can create as many of these as you want, but typically an app needs just one.
            ExportProvider = epf.CreateExportProvider();

            //ExportProvider.Register<IWindowManager>(_ => new WindowManager()).InstancePerLifetimeScope();
            //ExportProvider.Register<IEventAggregator>(_ => new EventAggregator()).InstancePerLifetimeScope();
            //ExportProvider.Register<IContainer>(_ => Container).InstancePerLifetimeScope();
            //ExportProvider.Register<BootstrapperBase>(_ => this).InstancePerLifetimeScope();
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindow>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return key == null ? GetExportedValue(ExportProvider, service) : GetExportedValue(ExportProvider, service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return null;
            //return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }

        private static object GetExportedValue(ExportProvider container, Type type)
        {
            // get a reference to the GetExportedValue<T> method
            MethodInfo methodInfo = container.GetType().GetMethods()
                                      .Where(d => d.Name == "GetExportedValue"
                                                  && d.GetParameters().Length == 0).First();

            // create an array of the generic types that the GetExportedValue<T> method expects
            Type[] genericTypeArray = new Type[] { type };

            // add the generic types to the method
            methodInfo = methodInfo.MakeGenericMethod(genericTypeArray);

            // invoke GetExportedValue<type>()
            return methodInfo.Invoke(container, null);
        }

        private static object GetExportedValue(ExportProvider container, Type type, string key)
        {
            // get a reference to the GetExportedValue<T> method
            MethodInfo methodInfo = container.GetType().GetMethods()
                                      .Where(d => d.Name == "GetExportedValue"
                                                  && d.GetParameters().Length == 1).First();

            // create an array of the generic types that the GetExportedValue<T> method expects
            Type[] genericTypeArray = new Type[] { type };

            // add the generic types to the method
            methodInfo = methodInfo.MakeGenericMethod(genericTypeArray);

            // invoke GetExportedValue<type>()
            return methodInfo.Invoke(container, new object[] { key });
        }
    }

    public interface MainWindow
    {

    }
}
