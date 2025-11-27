namespace Project.Infrastructure.DataSources.SqlDB.Implementations
{
    internal sealed class ProjectDB : GenericDB<Project>, IProject
    {
        public ProjectDB(GenericConfigurationSqlDbConnectionFactory<Project> connections) : base(connections) { }
    }
}
