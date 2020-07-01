using AccountAndJwt.Contracts.Models.Odata;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Controllers.Odata
{
	public class CustomersController : ODataController
	{
		private static readonly List<CustomerAm> _customers;


		static CustomersController()
		{
			_customers = new List<CustomerAm>
			{
				new CustomerAm
				{
					Id = 0,
					Name = "Jonier",
					HomeAddress = new AddressAm { City = "Redmond", Street = "156 AVE NE"},
					FavoriteAddresses = new List<AddressAm>
					{
						new AddressAm { City = "Redmond", Street = "256 AVE NE"},
						new AddressAm { City = "Redd", Street = "56 AVE NE"},
					},
					Order = new OrderAm { Title = "104m" },
					Orders = Enumerable.Range(0, 2).Select(e => new OrderAm { Title = "abc" + e }).ToList()
				},
				new CustomerAm
				{
					Id = 1,
					Name = "Sam",
					HomeAddress = new AddressAm { City = "Bellevue", Street = "Main St NE"},
					FavoriteAddresses = new List<AddressAm>
					{
						new AddressAm { City = "Red4ond", Street = "456 AVE NE"},
						new AddressAm { City = "Re4d", Street = "51 NE"},
					},
					Order = new OrderAm { Title = "Zhang" },
					Orders = Enumerable.Range(0, 2).Select(e => new OrderAm { Title = "xyz" + e }).ToList()
				},
				new CustomerAm
				{
					Id = 2,
					Name = "Peter",
					HomeAddress = new AddressAm {  City = "Hollewye", Street = "Main St NE"},
					FavoriteAddresses = new List<AddressAm>
					{
						new AddressAm { City = "R4mond", Street = "546 NE"},
						new AddressAm { City = "R4d", Street = "546 AVE"},
					},
					Order = new OrderAm { Title = "Jichan" },
					Orders = Enumerable.Range(0, 2).Select(e => new OrderAm { Title = "ijk" + e }).ToList()
				}
			};
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		[EnableQuery]
		public IEnumerable<CustomerAm> GetCustomers()
		{
			return _customers;
		}

		[EnableQuery]
		public CustomerAm GetCustomer(Int32 key)
		{
			return _customers.FirstOrDefault(p => p.Id == key);
		}
	}
}