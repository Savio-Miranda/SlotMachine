import numpy as np


class Matrix:
    def __init__(self, lines: int, colums: int, number_of_sprites: int,  round_points = 0, current_matrix = np.zeros_like((5, 3))):
        self.lines = lines
        self.columns = colums
        self.number_of_sprites = number_of_sprites
        self.current_matrix = current_matrix
        self.round_points = round_points
        self.bet_list = ["5", "10", "15", "20"]
        self.sequences = self.create_sequences()


    def random_structure(self):
        random_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.columns, self.lines)
        self.current_matrix = random_matrix
        self.rewards()


    # This method allow the game to get a pre-ordered pattern
    def ordered_structure(self):
        index = 0
        ordered_matrix = []

        for i in range(self.columns):
            suport_list = []
            index += 1

            for j in range(self.lines):
                suport_list.append(index)

            ordered_matrix.append(suport_list)

        ordered_matrix = np.array(ordered_matrix)
        self.current_matrix = ordered_matrix
        self.rewards()


    def create_sequences(self):
        sequences = {}
        for i in range(self.number_of_sprites):
            sequences.update({i: [i, i, i]})

        return sequences

    
    # retorna todos os padrões (linhas e colunas) dentro de uma matriz
    def rewards(self):
        matrix = self.current_matrix
        modified_matrix = np.reshape(matrix.ravel(order='F'), (3, 5), order='C')
        
        i = 0
        a = [matrix, modified_matrix]
        all_wins = []
        for i in range(len(a)):
            all_wins.append(self.rewards_match(a[i]))
            i += 1
        # exemplo de return => all wins:  [{}, {2: [1, 2, 3, 4]}]
        return all_wins


    # retorna os padrões dentro de uma matriz
    def rewards_match(self, matrix):
        win = {}
        matrix_size = len(matrix)


        for sequence in self.sequences:
            for i in range(matrix_size):
                # Store sizes of input array and sequence
                array_size = len(matrix[i])
                sequence_size = len(self.sequences[sequence])

                range_of_sequence = np.arange(sequence_size)

                # Create a 2D array of sliding indices across the entire length of input array.
                # Match up with the input sequence & get the matching starting indices.
                Match = (matrix[i][np.arange(array_size - sequence_size + 1)[:, None] + range_of_sequence] == sequence).all(1)

                if Match.any():
                    multiplier = int(np.array(matrix[i][0]))
                    obj = (np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[0].tolist(), multiplier)
                    win.update({i: obj})

        for k in win:
            pattern, image = len(win[k][0]), win[k][1]
            print(f"pattern: {pattern}   |   image: {image}")

            self.round_points += pattern * image

        return win
