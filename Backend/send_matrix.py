import numpy as np


class Matrix:
    def __init__(self, lines: int, colums: int, number_of_sprites: int, current_random_matrix = np.zeros_like((5, 3))):
        self.lines = lines
        self.columns = colums
        self.number_of_sprites = number_of_sprites
        self.current_random_matrix = current_random_matrix
        self.current_ordened_matrix = self.ordened_structure()
        self.sequences = self.create_sequences()


    def random_structure(self):
        random_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.columns, self.lines)
        self.current_random_matrix = random_matrix
        print("passou aqui")
        return random_matrix


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

        return ordened_matrix


    def create_sequences(self):
        sequences = {}
        for i in range(self.number_of_sprites):
            sequences.update({i: [i, i, i]})

        return sequences


    def rewards(self):
        matrix = self.current_random_matrix
        print(f"current matrix: {matrix}")
        
        #matrix = np.array([[1, 2, 3], [1, 2, 3], [1, 2, 3], [1, 2, 3], [1, 2, 3]]) TEST MATRIX
        matrix = np.reshape(matrix.ravel(order='F'), (3, 5), order='C')
        print(f"reshaped matrix:\n {matrix}")
        matrix_size = len(matrix)
        wins = {}
        
        

        #a = np.array([[1,2,3], [4,5,6]])
        #np.reshape(a, 6)
        

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
        
        if(len(wins) > 0):
            print("WINS:", wins)
        
        return wins

