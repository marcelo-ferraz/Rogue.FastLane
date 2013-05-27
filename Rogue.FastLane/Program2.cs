using System.Diagnostics;
using Rogue.FastLane;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;
using System.Reflection;

namespace Nhonho
{
    class Program2
    {
        #region
        static ReferenceNode<Pair, int> GetRef(int key, int count = 4, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, References = new ReferenceNode<Pair, int>[count], Parent = parent };
        }

        static ReferenceNode<Pair, int> GetRefVal(int key, int count = 4, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, Values = new ValueNode<Pair>[count], Parent = parent };
        }

        static ValueNode<Pair> GetVal(int key)
        {
            return new ValueNode<Pair>() { Value = new Pair() { Index = key } };
        }
        #endregion
        static void Main(string[] args)
        {
            var query =
                new UniqueKeyQuery<Pair, int>() { 
                     SelectKey = item => item.Index
                };

            var structure = 
                new OptmizedStructure<Pair>(query);

            structure.Add(new Pair() { Index = 3 });
            structure.Add(new Pair() { Index = 1 });
            structure.Add(new Pair() { Index = 2 });

            //var root = (ReferenceNode<Pair, int>)
            var root = 
                typeof(UniqueKeyQuery<,>)
                .MakeGenericType(typeof(Pair), typeof(int))
                .GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .Invoke(query, null);


            //foreach (var n in NodeFetchingMixins.IterateTroughLowestReferences(root, offsets))
            //{
            //    Debugger.Break();
            //}
            


			/*
			 * passos para solucionar o problema da inserção
			 * 1- iterar por todos os nodes, a partir do node encontrado
			 * 2- ver se existe alguma mudança na quantidade de niveis dos seletores
			 * 2.1- case tenha alguma mudança criar ou eliminar o nivel abaixo, arrumando as referencias
			 * 3- inserir um novo node no val array
			 * 3.1- caso o array tenha menos que o tamanho maximo, inserir nele
			 * 3.2- caso algum dos val arrays do node pai tenha menos que o tamanho maximo, inserir nele
			 * 3.3- caso algum dos val arrays de algum dos nodes tenha menos que o tamanho maximo, inserir nele
			 * 3b- procurar pelo primeiro array que ainda seja menor que o maximo e inserir nele
			 * 4- Colocar o valor no node certo e mover todos os outros uma posição, para mais, ou para menos 
			 * 4b- iterar e devolver os nodes mais baixos de referencia, os valueNodes para tornar a passagem mais facil
			 * 
			 * 
			 * com base na quantidade total e a de niveis, é possivel saber se algum node vai suportar esse item extra ou se é necessário que se faça mais um nivel
			 * planejamento
			 * aumento da capacidade
			 * mudanca de ponteiros
			 * inserçao
			 * */
			
			
			
			
            //var savior = new ProbableSavior() { Root = root };

            //savior.Save(new Pair() { Index = 6, Length=45 });
            //savior.Save(new Pair { Index = 14 });
            //savior.Save(new Pair { Index = 7 });
        }
		
		static void Save(ReferenceNode<Pair, int> item, int count)
		{
			
		}
    }
}
