using System;
using System.Linq;
using Estudos.EFCore.Tests.Data;
using Estudos.EFCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Estudos.EFCore.Tests
{
    public class InMemoryTest
    {
        [Fact(DisplayName = "inserir um departamento")]
        [Trait("InMemory", "inserir")]
        public void Deve_inserir_um_departamento()
        {
            // Arrange
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            // Act
            var inseridos = context.SaveChanges();

            // Assert
            Assert.Equal(1, inseridos);
        }

        [Fact(DisplayName = "Funções não impelemtadas")]
        [Trait("InMemory", "limitação de data")]
        public void Nao_implementado_funcoes_de_datas_para_o_provider_inmemory()
        {
            // Arrange
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            // Act
            var inseridos = context.SaveChanges();

            // Assert
            Action action = () => context
                .Departamentos
                .FirstOrDefault(p => EF.Functions.DateDiffDay(DateTime.Now, p.DataCadastro) > 0);

            Assert.Throws<InvalidOperationException>(action);
        }



        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("InMemoryTest")
                .Options;

            return new ApplicationContext(options);
        }
    }


}
