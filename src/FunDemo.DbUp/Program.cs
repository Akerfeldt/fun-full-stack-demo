namespace FunDemo.DbUp;

class Program
{
    static void Main(string[] args)
    {
        var dbUp = new DatabaseInitializer(Constants.MasterConnectionString, Constants.UserConnectionString);
        dbUp.InitializeDatabase();
    }
}
