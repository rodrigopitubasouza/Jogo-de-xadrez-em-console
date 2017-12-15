using System;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez {
    class PartidaDeXadrez {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> pecasCapturadas;
        public bool xeque { get; private set; }
        public Peca pecaVulneravelEnPassant { get; private set; }


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecaVulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca) {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas() {
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Rainha(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca,this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca,this));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Rainha(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preta,this));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preta,this));
        }


        public void realizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual)) {
                desfazMovimento(destino, origem, pecaCapturada);
                throw new tabuleiroException("Você não pode se colocar em xeque");
            }

            if (estaEmXeque(adversaria(jogadorAtual))) {
                xeque = true;
            }
            else
                xeque = false;

            if (testeXequeMate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                mudaJogador();
            }

            Peca p = tab.peca(destino);

            //jogada especial en Passant
            if (p is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2)) {
                pecaVulneravelEnPassant = p;
            }
            else
                pecaVulneravelEnPassant = null;


        }

        private void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.retirarPeca(origem);
            p.decrementaQtdMovimentos();
            if (pecaCapturada != null) {
                tab.colocarPeca(pecaCapturada, origem);
                pecasCapturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, destino);

            //jogada especial roque pequeno
            if (p is Rei && origem.coluna == destino.coluna + 2) {
                Posicao origemT = new Posicao(destino.linha, destino.coluna + 3);
                Posicao destinoT = new Posicao(destino.linha, destino.coluna + 1);
                desfazMovimento(destinoT, origemT, null);
            }

            //jogada especial roque grande
            if (p is Rei && origem.coluna == destino.coluna - 2) {
                Posicao origemT = new Posicao(destino.linha, destino.coluna - 4);
                Posicao destinoT = new Posicao(destino.linha, destino.coluna - 1);
                desfazMovimento(destinoT, origemT, null);
            }

            //jogada especial en passant
            if (p is Peao) {
                if (origem.coluna != destino.coluna && pecaCapturada == pecaVulneravelEnPassant) {
                    Peca peao = tab.retirarPeca(origem);
                    Posicao posP;
                    if (p.cor == Cor.Branca) {
                        posP = new Posicao(3, origem.coluna);
                    }
                    else
                        posP = new Posicao(4, origem.coluna);

                    tab.colocarPeca(peao,posP);
                }
            }
        }



        private Peca executaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                pecasCapturadas.Add(pecaCapturada);
            }

            //jogada especial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                executaMovimento(origemT, destinoT);
            }

            //jogada especial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                executaMovimento(origemT, destinoT);
            }

            //jogada especial en passant
            if (p is Peao) {
                if (origem.coluna != destino.coluna && pecaCapturada == null) {
                    Posicao posP;
                    if (p.cor == Cor.Branca) {
                        posP = new Posicao(destino.linha + 1, destino.coluna);

                    }
                    else
                        posP = new Posicao(destino.linha - 1, destino.coluna);

                    pecaCapturada = tab.retirarPeca(posP);
                    pecasCapturadas.Add(pecaCapturada);
                }
            }
            return pecaCapturada;
        }

        public HashSet<Peca> pecasCapturadasPorCor(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecasCapturadas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadasPorCor(cor));
            return aux;
        }

        private void mudaJogador() {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;
        }

        public bool estaEmXeque(Cor cor) {
            Peca R = rei(cor);
            if (R == null) {
                throw new tabuleiroException("O Rei não existe nesta cor");
            }
            bool[,] mat;
            foreach (Peca x in pecasEmJogo(adversaria(cor))) {
                mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequeMate(Cor cor) {

            if (!estaEmXeque(cor)) {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor)) {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++) {
                    for (int j = 0; j < tab.colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.posicao;
                            Posicao desfaz = new Posicao(i, j);
                            Peca capturada = executaMovimento(x.posicao, desfaz);
                            bool testaXeque = estaEmXeque(cor);
                            desfazMovimento(desfaz, origem, capturada);
                            if (!testaXeque) {
                                return false;
                            }
                        }
                    }

                }
            }
            return true;
        }
        private Peca rei(Cor cor) {
            foreach (Peca x in pecasEmJogo(cor)) {
                if (x is Rei)
                    return x;
            }
            return null;
        }

        private static Cor adversaria(Cor cor) {
            if (cor == Cor.Branca) {
                return Cor.Preta;
            }
            else
                return Cor.Branca;

        }

        public void validarPosicaoOrigem(Posicao pos) {
            if (tab.peca(pos) == null) {
                throw new tabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if (jogadorAtual != tab.peca(pos).cor) {
                throw new tabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis()) {
                throw new tabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao pos, bool[,] posicoesPossiveis) {
            if (!tab.posicaoValida(pos)) {
                throw new tabuleiroException("Não existe a posição digitada");
            }
            if (!posicoesPossiveis[pos.linha, pos.coluna]) {
                throw new tabuleiroException("Posição de destino inválida");
            }
        }


    }
}
