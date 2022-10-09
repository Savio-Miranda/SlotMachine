from msilib import sequence
import numpy as np


class Matrix:
    def __init__(self, horizontal_lines: int, vertical_lines: int):
        self.h = horizontal_lines
        self.v = vertical_lines
        self.current_matrix = self._structure()
        self.sequences = self.create_sequences()

    def _structure(self):
        new_matrix = np.random.randint(0, 10, 15).reshape(self.h, self.v)
        return new_matrix
    
    def rewards(self, seq):
        matrix, matrix_size = self.current_matrix, len(self.current_matrix)
        new_dict = {}
        print(matrix)
        
        for i in range(matrix_size):
            # Store sizes of input array and sequence
            array_size = matrix[i].size
            sequence_size = seq.size

            # Range of sequence
            range_of_sequence = np.arange(sequence_size)

            # Create a 2D array of sliding indices across the entire length of input array.
            # Match up with the input sequence & get the matching starting indices.
            Match = (matrix[i][np.arange(array_size - sequence_size + 1)[:, None] + range_of_sequence] == seq).all(1)
            print(Match)

            if Match.any():
                new_dict.update({i: np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[0]})
        
        if len(new_dict) > 0:
            return new_dict
        
        return new_dict

        # # Get the range of those indices as final output
        # if Match.any() > 0:
        #     return np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[i]
        # else:
        #     return []         # No match found
    
    def create_sequences(self):
        sequences = {}
        for i in range(self.current_matrix.size):
            sequences.update({i: [i, i, i]})

        return sequences


test = Matrix(3, 5)
test_sequence = np.array([2, 2])

# print(test.search_sequence_numpy(test_sequence))
print(test.rewards(test_sequence))