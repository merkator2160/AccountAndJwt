using System;
using System.Collections.Generic;

namespace AccountAndJwt.Contracts.Models.Odata
{
	public class CustomerAm
	{
		public Int32 Id { get; set; }
		public String Name { get; set; }
		public AddressAm HomeAddress { get; set; }
		public List<AddressAm> FavoriteAddresses { get; set; }
		public OrderAm Order { get; set; }
		public List<OrderAm> Orders { get; set; }
	}
}
