using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Scotia.Extensions;

using R5T.Suebia;


namespace R5T.Scotia
{
    public static class IConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds a user secret file from the custom secrets directory (%User%/Dropbox/Rivet/Data/Secrets).
        /// </summary>
        public static IConfigurationBuilder AddUserSecretsFileRivetLocation(this IConfigurationBuilder configurationBuilder, string secretsFileName)
        {
            // Build a service provider.
            var serviceProvider = new ServiceCollection()
                .AddUserSecretFilesRivetLocation()
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
