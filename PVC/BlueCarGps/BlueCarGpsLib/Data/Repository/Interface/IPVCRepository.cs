using BlueCarGpsLib.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Data.Repository.Interface
{

    public interface IPVCRepository
    {
        bool Create(PVC PVC);
        List<PVC> GetAll();
        bool Update(PVC PVC);
        IQueryable<PVC> Search(PVCSearchModel searchModel);
        bool DeleteById(int id);
    }
}
