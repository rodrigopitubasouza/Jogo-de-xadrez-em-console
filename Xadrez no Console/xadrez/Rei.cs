using System;
using tabuleiro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xadrez {
    class Rei : Peca {
        private PartidaDeXadrez partida;
        public Rei(Tabuleiro tab, Cor cor,PartidaDeXadrez partida) : base(cor,tab) {
            this.partida = partida;
        }

        public override string ToString() {
            return "R";
        }

        private bool testeTorreRoque(Posicao pos) {
            Peca p = tab.peca(pos);
            return p != null && p is Torre && p.cor == cor && p.qtdMovimento == 0;
        }

        private bool podeMover(Posicao pos) {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0,0);

            //acima
            pos.definirValores(posicao.linha - 1, posicao.coluna);
            if(tab.posicaoValida(pos) && podeMover(pos)){
                mat[pos.linha, pos.coluna] = true;
            }

            //ne
            pos.definirValores(posicao.linha - 1, posicao.coluna+1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            //direita
            pos.definirValores(posicao.linha, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            } 

            //se
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            //abaixo
            pos.definirValores(posicao.linha + 1, posicao.coluna);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            //so
            pos.definirValores(posicao.linha + 1, posicao.coluna -1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            //esquerda
            pos.definirValores(posicao.linha, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            //no
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }


            //jogada especial Roque 
            if (qtdMovimento == 0 && !partida.xeque) {
                //roque pequeno
                Posicao PosicaoTorre = new Posicao(posicao.linha, posicao.coluna + 3);
                if (testeTorreRoque(PosicaoTorre)) {
                    Posicao p1 = new Posicao(posicao.linha, posicao.coluna + 1);
                    Posicao p2 = new Posicao(posicao.linha, posicao.coluna + 2 );
                    if (tab.peca(p1) == null && tab.peca(p2) == null) {
                        mat[posicao.linha, posicao.coluna + 2] = true; 
                    }

                }
                //Roque Grande
                Posicao PosicaoTorre2 = new Posicao(posicao.linha, posicao.coluna - 4);
                if (testeTorreRoque(PosicaoTorre2)) {
                    Posicao p1 = new Posicao(posicao.linha, posicao.coluna - 1);
                    Posicao p2 = new Posicao(posicao.linha, posicao.coluna - 2);
                    Posicao p3 = new Posicao(posicao.linha, posicao.coluna - 3);
                    if (tab.peca(p1) == null && tab.peca(p2) == null && tab.peca(p3) == null){
                        mat[posicao.linha, posicao.coluna - 2] = true;
                    }

                }
            }

            return mat; 



        }
    }
}
