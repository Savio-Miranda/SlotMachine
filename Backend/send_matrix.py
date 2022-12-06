import numpy as np


class Matrix:
    def __init__(self, horizontal_lines: int, vertical_lines: int, number_of_sprites: int):
        self.h = horizontal_lines
        self.v = vertical_lines
        self.number_of_sprites = number_of_sprites
        self.current_matrix = self.structure()
        self.sequences = self.create_sequences()

    def structure(self):
        new_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.h, self.v)
        return new_matrix.tolist()
    
    def create_sequences(self):
        sequences = {}
        for i in range(self.number_of_sprites):
            sequences.update({i: [i, i, i]})

        return sequences

    def rewards(self):
        matrix, matrix_size = self.current_matrix, len(self.current_matrix)
        matrix = np.array([[1, 2, 2, 2, 2], [1, 1, 3, 3, 3], [2, 9, 9, 9, 2]]) # test matrix
        print("matrix:\n", matrix)
        wins = {}

        for sequence in self.sequences:
            for i in range(matrix_size):
                # Store sizes of input array and sequence
                array_size = matrix[i].size
                sequence_size = len(self.sequences[sequence])

                # Range of sequence
                range_of_sequence = np.arange(sequence_size)

                # Create a 2D array of sliding indices across the entire length of input array.
                # Match up with the input sequence & get the matching starting indices.
                Match = (matrix[i][np.arange(array_size - sequence_size + 1)[:, None]+ range_of_sequence] == sequence).all(1)

                if Match.any():
                    wins.update({i: np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[0].tolist()})
        
        return wins


# test = Matrix(3, 5, 11)
# #test_sequence = np.array([2, 2, 2])
# #print("test.rewards(test_sequence): ", test.rewards(test_sequence))

# print("test.rewards: ", test.rewards())