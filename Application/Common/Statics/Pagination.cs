using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Statics;
public static class Pagination
{
    public static List<T> GetList<T>(IEnumerable<T> query, int? page = null, int? perPage = null) {
        var pageList =new List<T>();
        if (perPage.Value==0)
          pageList = query.ToList();
        else
         pageList= query.Skip((page.Value - 1) * perPage.Value).Take(perPage.Value).ToList();

        return pageList;
    }
}
