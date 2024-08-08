# WebApiApplication With Ado.net Using Dapper
webapi  Sample Application with ADO.net using Dapper
Step 1
Create new webapi project from visual studio
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/0a9061ed-ff66-4a68-b4e8-06204d5901af)


 
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/bf14bbcb-4788-4b76-a8ef-f567b0044c33)


Figure 2 – Select Web API Template

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/d402f1d8-93e8-4b86-8c78-ec6fbdbb59d6)


 
Figure 3 – Give project name and select location for saving project files
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/be25de6c-3adf-4665-8574-9c8ec3b66bc9)


 
Figure 4 – Select Faremwork (.Net6) 
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/279a3f51-4c58-4550-b299-f87583f28e72)


Figure 5 – Project created you can check on the Solution explorer Right Side
If getting build errors Install Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.Tools, MySql.Data Using nuget package manager

To enable cross-origin requests on the apis add the below code In the middleware on program.cs file
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((hosts) => true));
});
Add Connection string details on the program.cs file
// Read the connection string from appsettings.
string dbConnectionString = builder.Configuration.GetConnectionString("AppConnection");

builder.Services.AddTransient<IDbConnection>(sql => new MySqlConnection(dbConnectionString));
Here we need to add the Mysql.Data in to the application to resolve the error.
Add Class Library for Business logic Layer



Step 2
Create New Class Library for business logic related files to save.
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/62de163d-ba49-455b-b4be-0cb316dddee4)


Figure 1 – Select class library from project template list
 ![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/5ddf8ede-c836-42ec-9de2-bd14bf02e503)


Figure 2 – Giving Proper name and selecting proper Folder for class library
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/5aaf6fc9-952c-4299-b534-6d786275a5af)


Figure 3 – Select Framework (.Net6)
If getting build errors Install Microsoft.Extensions.DependencyInjection.Abstractions using nuget package manager

Step 3 
Create Class library for Data Access 
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/b4509bc2-d487-494c-bbfb-e8dccea5f127)


  Figure 1 – Select class library from project template list
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/52af3c84-ec66-4121-a60b-a53183cd8ae2)


Figure 2 – Giving Proper name and selecting proper Folder for class library

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/1e8d93ad-15cb-4ae0-acac-da75ccbab314)

Figure 3 – Select Framework (.Net6)
If getting build errors  Install Microsoft.EntityFrameworkCore ,Pomelo.EntityFrameworkCore.MySql, Microsoft.EntityFrameworkCore.Relational, on the dataccess layer using nuget package manager
After creating projects we can start with creating Db Connectionstring 
Add New abstract class named ConnectionBase in the folder for database based functionalities using IDbConnection Interface.

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/fc9f8881-5db7-4a49-899b-96716e28b26f)


using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Infrastructure
{
    public abstract class ConnectionBase : IDbConnection
    {
        readonly string injectedConnectionString;
        public ConnectionBase(string connectionString)
        {
            this.injectedConnectionString = connectionString;
        }

        protected ConnectionBase(IDbConnection connection)
        {
            Connection = connection;
        }

        private IDbConnection Connection { get; set; }

        // Verbose but necessary implementation of IDbConnection:
        #region "IDbConnection implementation"

        public string ConnectionString
        {
            get
            {
                return Connection.ConnectionString;
            }

            set
            {
                Connection.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return Connection.ConnectionTimeout;
            }
        }

        public string Database
        {
            get
            {
                return Connection.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return Connection.State;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Connection.BeginTransaction(isolationLevel);
        }

        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            Connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return Connection.CreateCommand();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public void Open()
        {
            Connection.Open();
        }

        #endregion

        protected IDbConnection GetConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("conneciton string is null or empty");

            return new MySqlConnection(connectionString);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                return GetConnection(Connection.ConnectionString);
            }
            else
            {
                if (Connection == null)
                    throw new ArgumentNullException("conneciton unable to inject");

                return Connection;

            }



        }

    }
}

Here we need to add the Mysql.Data in to the application to resolve the error.

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/f3d3888f-ca67-4e15-8aa6-c2e5256b55fd)

You can create new classes for using in application controllers in Model folder 

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/c058aa10-1d57-40a2-aa01-4bc16e6c1b9a)

Then Create Interface for Repository files to access from bussinesslogic layer ICompanyrepositry.

![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/a3b991e9-f20e-4e5e-a236-6223168b1a6d)


Then Create Repository files to access data from database using Entity Framework named Companyrepositry.

Then We need to create buissiness classes .
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/2e3b5b2e-bad4-4cd7-ba12-79150669f6f0)

Create business files (Interface and its implementation) as show in the above figure.
![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/c71a5f8d-ea2c-4ef6-9142-6bb42e87d11b)


Business login Implementation class.
Used IRepository interface for getting data from database to the business layer.
The below code is populating entity class from the company model getting from the controller method.
    public async Task<string> CreateCompany(CompanyModel param)
    {
        var companyData = new CompanyEntity()
        {
            CompanyAddress = param.CompanyAddress,
            CompanyCity = param.CompanyCity,
            CompanyName = param.CompanyName,
            CompanyPhone = param.CompanyPhone,
            CompanyCountry = param.CompanyCountry,
            CompanyState = param.CompanyState,
            CompanyDescription = param.CompanyDescription,
            CompanyEmail = param.CompanyEmail,
            LandMark = param.LandMark,
            Place = param.Place,
            CreatedBy = param.CreatedBy,
        };
        return await _companyRepository.CreateCompany(companyData);
    }


![image](https://github.com/vipinvijayan/WebApiApplicationWithADO.NetUsingDapper/assets/8413745/1167bccc-5319-43d7-8f89-6cbc75e90251)
 
Interface class for the business logic layer.



Then we can inject repository layer and business layer to our api application middle ware for running the application.
Code in Program.cs
using CliniqonFindFriendsAppBuisinessLogic;
using CliniqonFindFriendsAppBuisinessLogic.Implementation;
using CliniqonFindFriendsAppBuisinessLogic.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//To allow cross-origin request.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((hosts) => true));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Read the connection string from appsettings.
string dbConnectionString = builder.Configuration.GetConnectionString("AppConnection");

builder.Services.AddTransient<IDbConnection>(sql => new MySqlConnection(dbConnectionString));

builder.Services.AddScoped<ICompanyB, CompanyB>(); // Custom Buisiness layer injection
builder.Services.AddRepositoryDependencies(); //Repository Layer Injection

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


