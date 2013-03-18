neuron
======

neuron is a program that allows you to build an artificial neural network 
with different topologies and train it on data using backpropagation. It
started off after attending an online course on Neural Networks on
Coursera by Prof. Geoffrey Hinton.

This program is able to use linear and logarithmic neurons. It applies
biases for you automatically so you don't have to worry about them. You 
can also save your neural network and data into a single file, making 
retrieval quick and easy.

The idea here is to enable fast experimentation of neural networks and
the effect of different topologies and different initial weights on
training convergence.

TODO
====
- add ability to view actual values of nodes
- add softmax layer type
- add mirror and reflector layers type (for autoencoders)
- clean up code; detach blueblocks from codebase
- implement export/import workspace to PMML (http://www.dmg.org/v4-1/NeuralNetwork.html)
- add automatic regularization
- add more training controls (learning rate, weight decay, momentum)
- add visualization for training (training/cross validation error/entropy)
- make manual node update more efficient
- add ability to define node states with bitmaps/pictures
- add ability to visualize weight matrices
- perform batch training (large matrix operations) with CUDA/OpenCL
- add ability to train/pretrain with contrastive divergence
- add ability to train recurrent neural nets (by forming cycles)
  - add ability to define serial data
  - add ability to visualize network in serial form
  - add ability to visualize initial states
  - add ability to apply serial data from external sources
- add radial basis and other activation functions
