namespace Project.Infrastructure.DataSources.SqlDB.Implementations
{
    internal sealed class ProjectDB : GenericDB<ProjectDatabase>, IProjectDb
    {
        public ProjectDB(GenericConfigurationSqlDbConnectionFactory<ProjectDatabase> connections) : base(connections) { }
    }
}
