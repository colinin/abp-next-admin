using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
public class DataAccessFilterGroup
{
    public List<DataAccessFilterGroup> Groups { get; set; }
    public List<DataAccessFilterRule> Rules { get; set; }
    public DataAccessFilterLogic Logic { get; set; }

    public DataAccessFilterGroup() : this(DataAccessFilterLogic.And)
    {
    }

    public DataAccessFilterGroup(DataAccessFilterLogic logic = DataAccessFilterLogic.And)
    {
        Logic = logic;
        Rules = new List<DataAccessFilterRule>();
        Groups = new List<DataAccessFilterGroup>();
    }

    public DataAccessFilterGroup AddRule(DataAccessFilterRule rule)
    {
        if (Rules.All(m => !m.Equals(rule)))
        {
            Rules.Add(rule);
        }

        return this;
    }

    public DataAccessFilterGroup AddRule(string field, object value, DataAccessFilterOperate operate = DataAccessFilterOperate.Equal)
    {
        return AddRule(new DataAccessFilterRule(field, value, operate));
    }
}