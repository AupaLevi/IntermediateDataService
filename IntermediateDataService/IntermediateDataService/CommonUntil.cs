using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class CommonUntil
    {
        private string timestamp;
        private int year;
        private int month;
        private int day;

        public string gettimestamp()
        {
            this.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            return this.timestamp;
        }

        public int getYear()
        {
            this.year = (Int16)DateTime.Now.Year;

            return this.year;
        }

        public int getDay()
        {
            this.day = (Int16)DateTime.Now.Day;

            return this.day;
        }

        public int getMonth()
        {
            this.month = (Int16)DateTime.Now.Month;

            return this.month;
        }

    }
}

   