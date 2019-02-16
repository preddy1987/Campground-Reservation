using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeography.Models;

namespace WorldGeography.DAL
{
    public class LanguageSqlDAL
    {
        private string connectionString;
        private const string SQL_InsertLanguage = @"insert into countrylanguage values (@countryCode, @language, @isOfficial, @percentage);";
        private const string SQL_LanguagesByCountry = @"select *  from countrylanguage  where countrycode = @countryCode and isofficial = @isOfficial;";
        private const string SQL_DeleteLanguage = "DELETE FROM countryLanguage WHERE countryCode = @countryCode AND language = @language";



        public LanguageSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public List<Language> GetLanguages(string countryCode, bool officialOnly)
        {
            throw new NotImplementedException();
        }

        public bool AddNewLanguage(Language newLanguage)
        {
            throw new NotImplementedException();
        }

        public bool RemoveLanguage(Language deadLanguage)
        {
            throw new NotImplementedException();
        }
    }
}
