﻿namespace CityInfo.Core
{
   public interface IMailService
    {
        void Send(string subject, string message);
    }
}
