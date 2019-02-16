using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeography.Models;

namespace WorldGeography.DAL
{
    public class CitySqlDAL
    {
        private const string SQL_CountryCodeCities = "SELECT city.id, city.name, city.countrycode, city.district, city.population FROM city WHERE countryCode = @countrycode ORDER BY city.district, city.name;";
        private string connectionString;

        public CitySqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }


        public List<City> GetCitiesByCountryCode(string countryCode)
        {
            throw new NotImplementedException();
        }

    }
}
