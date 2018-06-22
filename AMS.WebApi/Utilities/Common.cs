using AMS.ApplicationCore.Entities;
using AMS.Infrastructure.Repository;
using AMS.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.WebApi.Utilities
{
	public static class Common
	{
		public static SettingData GetSettingData()
		{
			SettingData data = new SettingData();
			try
			{
				List<Setting> setting = AccountRepository.AppSetting();

				data.Goal = setting.Any(s => s.Id.Equals(1) && s.StartDate <= DateTime.Now && DateTime.Now <= s.EndDate);
				data.Rate = setting.Any(s => s.Id.Equals(2) && s.StartDate <= DateTime.Now && DateTime.Now <= s.EndDate);
				data.Review = setting.Any(s => s.Id.Equals(3) && s.StartDate <= DateTime.Now && DateTime.Now <= s.EndDate);
				return data;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}