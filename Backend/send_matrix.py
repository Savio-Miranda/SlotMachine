import numpy as np


class Matrix:
    def __init__(self, lines: int, colums: int, number_of_sprites: int, current_matrix = np.zeros_like((5, 3))):
        self.lines = lines
        self.columns = colums
        self.number_of_sprites = number_of_sprites
        self.current_matrix = current_matrix
        self.sequences = self.create_sequences()


    def random_structure(self):
        random_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.columns, self.lines)
        self.current_matrix = random_matrix


    # This method allow the game to get a pre-ordened pattern
    def ordened_structure(self):
        index = 0
        ordened_matrix = []

        for i in range(self.columns):
            suport_list = []
            index += 1

            for j in range(self.lines):
                suport_list.append(index)

            ordened_matrix.append(suport_list)

        ordened_matrix = np.array(ordened_matrix)
        self.current_matrix = ordened_matrix


    def create_sequences(self):
        sequences = {}
        for i in range(self.number_of_sprites):
            sequences.update({i: [i, i, i]})

        return sequences


    def rewards(self):
        matrix = self.current_matrix
        modified_matrix = np.reshape(matrix.ravel(order='F'), (3, 5), order='C')
        
        i = 0
        a = [matrix, modified_matrix]
        all_wins = []
        for i in range(len(a)):
            all_wins.append(self.rewards_match(a[i]))
            i += 1
        return all_wins

    def rewards_match(self, matrix):
        wins = {}
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
                    wins.update({i:np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[0].tolist()})
        
        return wins
