﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Estudos.EFCore.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public Departamento(){}

        // obrigatório que o nome seja _lazyLoader
        private Action<object, string> _lazyLoader { get; set; }
        private Departamento(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private List<Funcionario> _funcionarios;
        public List<Funcionario> Funcionarios
        {
            get
            {
                _lazyLoader?.Invoke(this, nameof(Funcionarios));

                return _funcionarios;
            }
            set => _funcionarios = value;
        }

        //private ILazyLoader _lazyLoader { get; set; }
        //private Departamento(ILazyLoader lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}

        //private List<Funcionario> _funcionarios;
        //public List<Funcionario> Funcionarios
        //{
        //    get => _lazyLoader.Load(this, ref _funcionarios);
        //    set => _funcionarios = value;
        //}
    }
}