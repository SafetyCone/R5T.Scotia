using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Alamania;
using R5T.Alamania.Bulgaria;
using R5T.Bulgaria;
using R5T.Bulgaria.Default.Local;
using R5T.Costobocia;
using R5T.Costobocia.Default;
using R5T.Lombardy;
using R5T.Suebia;
using R5T.Suebia.Alamania;
using R5T.Visigothia;
using R5T.Visigothia.Default.Local;


namespace R5T.Scotia
{
    public static class IConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds a user secret file from the custom secrets directory (%User%/Dropbox/Rivet/Data/Secrets).
        /// </summary>
        public static IConfigurationBuilder AddCustomUserSecretsFile(this IConfigurationBuilder configurationBuilder, string secretsFileName)
        {
            // Build a service provider.
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IStringlyTypedPathOperator, StringlyTypedPathOperator>()

                .AddSingleton<IUserProfileDirectoryPathProvider, DefaultLocalUserProfileDirectoryPathProvider>()
                .AddSingleton<IDropboxDirectoryPathProvider, DefaultLocalDropboxDirectoryPathProvider>()
                .AddSingleton<IOrganizationsStringlyTypedPathOperator, DefaultOrganizationsStringlyTypedPathOperator>()
                .AddSingleton<IOrganizationDirectoryNameProvider, DefaultOrganizationDirectoryNameProvider>()
                .AddSingleton<IOrganizationStringlyTypedPathOperator, DefaultOrganizationStringlyTypedPathOperator>()
                .AddSingleton<IRivetOrganizationDirectoryPathProvider, BulgariaRivetOrganizationDirectoryPathProvider>()
                .AddSingleton<ISecretsDirectoryPathProvider, AlamaniaSecretsDirectoryPathProvider>()
                .AddSingleton<ISecretsFilePathProvider, AlamaniaSecretsFilePathProvider>()

                .BuildServiceProvider();

            // Get the secrets file path provider.
            var secretsFilePathProvider = serviceProvider.GetRequiredService<ISecretsFilePathProvider>();

            // Get the path of the secrets file in the custom 
            var secretsFilePath = secretsFilePathProvider.GetSecretsFilePath(secretsFileName);

            configurationBuilder
                .AddJsonFile(secretsFilePath);

            return configurationBuilder;
        }
    }
}
