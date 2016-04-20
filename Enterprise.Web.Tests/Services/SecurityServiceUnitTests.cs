﻿using System;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprise.Web.Tests.Services
{
    [TestClass]
    public class SecurityServiceUnitTests
    {
        [TestInitialize]
        public void Init()
        {
            AutofacConfig.RegisterAutofac();
        }
    }
}