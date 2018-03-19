using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorterKicadLibrary
{
  class Item
  {
    public String Name;
    public IList<string> Content= new List<string>();

		internal void SetName()
		{
			Name = Content.ElementAt(1).Replace("# ", "");
		}
	}
}
