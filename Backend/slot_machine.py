import numpy as np


class SlotMachine:
    def __init__(self, lines: int, columns: int, number_of_sprites: int,  scores = 0, current_matrix = np.zeros_like((5, 3)), name = "", credits = 50, bet = 5):
        # Machines engine
        self.lines = lines
        self.columns = columns
        self.number_of_sprites = number_of_sprites
        self.reset_machine = False

        # Machine data
        self.bet_list = [5, 10, 15, 20]
        self.bet = bet
        self.current_matrix = current_matrix
        self.scores = scores
        self.credits = credits
        self.sequences = self.create_sequences()

    
    def start_structure(self):
        new_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.columns, self.lines)
        self.current_matrix = np.zeros_like(new_matrix)
        self.slot_machine_data()
        self.reset_machine = False
        return self.reset_machine


    def random_structure(self):
        random_matrix = np.random.randint(0, self.number_of_sprites, 15).reshape(self.columns, self.lines)
        self.current_matrix = random_matrix
        self.rewards()


    # This method allow the game to get a pre-ordered pattern
    def ordered_structure(self):
        
        # TESTING
        # ordered_matrix = [[1, 0, 1], [1, 1, 1], [1, 0, 11], [1, 0, 11], [1, 0, 11]]

        index = 0
        ordered_matrix = []

        for i in range(self.columns):
            support_list = []
            index += 1

            for j in range(self.lines):
                support_list.append(index)

            ordered_matrix.append(support_list)

        ordered_matrix = np.array(ordered_matrix)
        self.current_matrix = ordered_matrix
        self.rewards()


    def create_sequences(self):
        sequences = {}
        for i in range(self.number_of_sprites):
            sequences.update({i: [i, i, i]})

        return sequences
    

    # Returns all rewards found inside a matrix
    def rewards(self):
        matrix = self.current_matrix
        modified_matrix = np.reshape(matrix.ravel(order='F'), (3, 5), order='C')
        
        i = 0
        a = [matrix, modified_matrix]
        all_wins = []
        for i in range(len(a)):
            all_wins.append(self.rewards_match(a[i]))
            i += 1
        
        # Example: => all wins:  [{}, {2: [1, 2, 3, 4]}]
        self.credits += self.scores
        return all_wins


    # Returns all patterns inside a matrix
    def rewards_match(self, matrix):
        win = {}
        matrix_size = len(matrix)

        for sequence in self.sequences:
            for i in range(matrix_size):
                # Store input array size and sequence size
                array_size = len(matrix[i])
                sequence_size = len(self.sequences[sequence])
                range_of_sequence = np.arange(sequence_size)

                # Create a 2D array of sliding indices across the entire length of input array.
                # Match up with the input sequence & get the matching starting indices.
                Match = (matrix[i][np.arange(array_size - sequence_size + 1)[:, None] + range_of_sequence] == sequence).all(1)

                if Match.any():
                    multiplier = int(np.bincount(matrix[i]).argmax())
                    obj = (np.where(np.convolve(Match, np.ones(sequence_size, dtype=int)) > 0)[0].tolist(), multiplier)
                    win.update({i: obj})

        # Adding the acquired sprite based points this round on credits and round_points
        # Both variables (credits and round_points will be shown individually on Slot Machine's screen in Unity)
        for k in win:
            pattern, image = len(win[k][0]), win[k][1]
            print(f"pattern: {pattern}, image: {image}")
            self.scores += pattern * image * self.bet

        return win

    def slot_machine_data(self):
        obj = {"matrix": self.current_matrix.tolist(), "scores": self.scores, "credits": self.credits}
        self.scores = 0
        return obj

    def game_over(self):
        self.credits = 50
        self.start_structure()
        return
