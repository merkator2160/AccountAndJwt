using AccountAndJwt.Ui.Models.Radzen;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("lineChart")]
    public partial class LineChart
    {
        private Boolean _smooth;



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private String FormatAsUsd(Object value)
        {
            return ((Double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }


        // DATA ///////////////////////////////////////////////////////////////////////////////////
        private readonly DataItem[] _revenue2019 = new DataItem[]
        {
            new DataItem
            {
                Date = DateTime.Parse("2019-01-01"),
                Revenue = 234000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-02-01"),
                Revenue = 269000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-03-01"),
                Revenue = 233000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-04-01"),
                Revenue = 244000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-05-01"),
                Revenue = 214000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-06-01"),
                Revenue = 253000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-07-01"),
                Revenue = 274000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-08-01"),
                Revenue = 284000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-09-01"),
                Revenue = 273000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-10-01"),
                Revenue = 282000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-11-01"),
                Revenue = 289000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-12-01"),
                Revenue = 294000
            }
        };

        private readonly DataItem[] _revenue2020 = new DataItem[]
        {
            new DataItem
            {
                Date = DateTime.Parse("2019-01-01"),
                Revenue = 334000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-02-01"),
                Revenue = 369000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-03-01"),
                Revenue = 333000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-04-01"),
                Revenue = 344000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-05-01"),
                Revenue = 314000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-06-01"),
                Revenue = 353000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-07-01"),
                Revenue = 374000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-08-01"),
                Revenue = 384000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-09-01"),
                Revenue = 373000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-10-01"),
                Revenue = 382000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-11-01"),
                Revenue = 389000
            },
            new DataItem
            {
                Date = DateTime.Parse("2019-12-01"),
                Revenue = 394000
            }
        };
    }
}