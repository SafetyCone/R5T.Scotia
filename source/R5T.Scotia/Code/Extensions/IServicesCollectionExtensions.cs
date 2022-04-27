using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Alamania;
using R5T.Alamania.Bulgaria;
using R5T.Bulgaria;
using R5T.Bulgaria.Default.Local;
using R5T.Dacia;
using R5T.Dacia.Extensions;
using R5T.Costobocia;
using R5T.Costobocia.Default;
using R5T.Lombardy;
using R5T.Suebia;
using R5T.Suebia.Alamania;
using R5T.Suebia.Default;
using R5T.Visigothia;
using R5T.Visigothia.Default.Local;


namespace R5T.Scotia.Extensions
{
    public static class IServicesCollectionExtensions
    {
        public static
            (
            IServiceAction<IDropboxDirectoryPathProvider> DropboxDirectoryPathProviderAction,
            IServiceAction<IOrganizationDirectoryNameProvider> OrganizationDirectoryNameProviderAction,
            IServiceAction<IOrganizationsStringlyTypedPathOperator> OrganizationsStringlyTypedPathOperatorAction,
            IServiceAction<IOrganizationStringlyTypedPathOperator> OrganizationStringlyTypedPathOperatorAction,
            IServiceAction<IRivetOrganizationDirectoryPathProvider> RivetOrganizationDirectoryPathProviderAction,
            IServiceAction<IUserProfileDirectoryPathProvider> UserProfileDirectoryPathProviderAction
            )
        AddRivetOrganizationDirectoryPathProviderAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            // 0.
            var organizationDirectoryNameProvider = services.AddOrganizationDirectoryNameProviderAction_Old();
            var userProfileDirectoryPathProviderAction = services.AddLocalUserProfileDirectoryPathProviderAction_Old();

            // 1.
            var dropboxDirectoryPathProviderAction = services.AddLocalDropboxDirectoryPathProviderAction_Old(
                stringlyTypedPathOperatorAction,
                userProfileDirectoryPathProviderAction);
            var organizationsStringlyTypedPathOperatorAction = services.AddOrganizationsStringlyTypedPathOperatorAction_Old(
                stringlyTypedPathOperatorAction);

            // 2.
            var organizationStringlyTypedPathOperatorAction = services.AddOrganizationStringlyTypedPathOperatorAction_Old(
                organizationDirectoryNameProvider,
                organizationsStringlyTypedPathOperatorAction,
                stringlyTypedPathOperatorAction);

            // 3.
            var rivetOrganizationDirectoryPathProviderAction = services.AddRivetOrganizationDirectoryPathProviderAction_Old(
                dropboxDirectoryPathProviderAction,
                organizationStringlyTypedPathOperatorAction);

            services
                .Run(rivetOrganizationDirectoryPathProviderAction)
                ;

            return
                (
                dropboxDirectoryPathProviderAction,
                organizationDirectoryNameProvider,
                organizationsStringlyTypedPathOperatorAction,
                organizationStringlyTypedPathOperatorAction,
                rivetOrganizationDirectoryPathProviderAction,
                userProfileDirectoryPathProviderAction
                );
        }

        public static 
            (
            IServiceAction<IRivetOrganizationSecretsDirectoryPathProvider> RivetOrganizationSecretsDirectoryPathProviderAction,
            IServiceAction<ISecretsDirectoryFilePathProvider> SecretsDirectoryFilePathProviderAction,
            IServiceAction<ISecretsDirectoryPathProvider> SecretsDirectoryPathProviderAction,
            (
            IServiceAction<IDropboxDirectoryPathProvider> DropboxDirectoryPathProviderAction,
            IServiceAction<IOrganizationDirectoryNameProvider> OrganizationDirectoryNameProviderAction,
            IServiceAction<IOrganizationsStringlyTypedPathOperator> OrganizationsStringlyTypedPathOperatorAction,
            IServiceAction<IOrganizationStringlyTypedPathOperator> OrganizationStringlyTypedPathOperatorAction,
            IServiceAction<IRivetOrganizationDirectoryPathProvider> RivetOrganizationDirectoryPathProviderAction,
            IServiceAction<IUserProfileDirectoryPathProvider> UserProfileDirectoryPathProviderAction
            ) RivetOrganizationDirectoryPathProviderAction
            )
        AddRivetOrganizationSecretDirectoryFilePathProviderAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            // -1.
//#pragma warning disable IDE0042 // Deconstruct variable declaration
            var rivetOrganizationDirectoryPathProviderAction = services.AddRivetOrganizationDirectoryPathProviderAction(
                stringlyTypedPathOperatorAction);
//#pragma warning restore IDE0042 // Deconstruct variable declaration

            // 1.
            var rivetOrganizationSecretsDirectoryPathProviderAction = services.AddRivetOrganizationSecretsDirectoryPathProviderAction_Old(
                rivetOrganizationDirectoryPathProviderAction.RivetOrganizationDirectoryPathProviderAction,
                stringlyTypedPathOperatorAction);

            // 2.
            var secretsDirectoryPathProviderAction = services.ForwardRivetOrganizationSecretsDirectoryPathProviderAsSecretsDirectoryPathProviderAction_Old(
                rivetOrganizationSecretsDirectoryPathProviderAction);

            // 3.
            var secretsDirectoryFilePathProviderAction = services.AddSecretsDirectoryFilePathProviderAction_Old(
                secretsDirectoryPathProviderAction,
                stringlyTypedPathOperatorAction);

            services
                .Run(secretsDirectoryFilePathProviderAction)
                ;

            return
                (
                rivetOrganizationSecretsDirectoryPathProviderAction,
                secretsDirectoryFilePathProviderAction,
                secretsDirectoryPathProviderAction,
                rivetOrganizationDirectoryPathProviderAction
                );
        }


        public static IServiceCollection AddRivetOrganizationSecretsDirectoyPathProviderServiceDependencies(this IServiceCollection services)
        {
            services
                .TryAddSingletonFluent<IStringlyTypedPathOperator, StringlyTypedPathOperator>() // Add if not already present.

                .AddSingleton<IUserProfileDirectoryPathProvider, LocalUserProfileDirectoryPathProvider>()
                .AddSingleton<IDropboxDirectoryPathProvider, LocalDropboxDirectoryPathProvider>()
                .AddSingleton<IOrganizationsStringlyTypedPathOperator, OrganizationsStringlyTypedPathOperator>()
                .AddSingleton<IOrganizationDirectoryNameProvider, OrganizationDirectoryNameProvider>()
                .AddSingleton<IOrganizationStringlyTypedPathOperator, OrganizationStringlyTypedPathOperator>()
                .AddSingleton<IRivetOrganizationDirectoryPathProvider, RivetOrganizationDirectoryPathProvider>()
                ;

            return services;
        }

        public static IServiceCollection AddRivetOrganizationSecretFilesLocation(this IServiceCollection services)
        {
            services
                .AddRivetOrganizationSecretsDirectoyPathProviderServiceDependencies()
                .AddSingleton<ISecretsDirectoryPathProvider, RivetOrganizationSecretsDirectoryPathProvider>()
                .AddSingleton<ISecretsDirectoryFilePathProvider, SecretsDirectoryFilePathProvider>()
                ;

            return services;
        }
    }
}
