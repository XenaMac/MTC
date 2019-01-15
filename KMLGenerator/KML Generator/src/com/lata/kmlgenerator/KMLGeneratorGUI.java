package com.lata.kmlgenerator;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import java.awt.BorderLayout;
import java.awt.FlowLayout;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.JTextPane;

public class KMLGeneratorGUI {

	private JFrame frmKmlGenerator;
	private final JPanel panel = new JPanel();
	private JTextField textField;
	private KMLGenerator kMLGenerator;
	private JTextField textField_1;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					KMLGeneratorGUI window = new KMLGeneratorGUI();
					window.frmKmlGenerator.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the application.
	 */
	public KMLGeneratorGUI() {
		initialize();
	}

	/**
	 * Initialize the contents of the frame.
	 */
	private void initialize() {
		frmKmlGenerator = new JFrame();
		frmKmlGenerator.setTitle("KML Generator");
		frmKmlGenerator.setBounds(100, 100, 375, 175);
		frmKmlGenerator.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frmKmlGenerator.getContentPane().add(panel, BorderLayout.CENTER);
		panel.setLayout(new FlowLayout(FlowLayout.CENTER, 5, 5));
		
		JLabel lblFolder = new JLabel("Log Directory:");
		panel.add(lblFolder);
		textField = new JTextField();
		panel.add(textField);
		textField.setColumns(30);
		JLabel lblModem = new JLabel("Modem Name:");
		panel.add(lblModem);
		textField_1 = new JTextField();
		panel.add(textField_1);
		textField_1.setColumns(30);
		
		JButton btnNewButton = new JButton("Generate KML");
		btnNewButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				kMLGenerator = new KMLGenerator(textField.getText(), textField_1.getText());
				String errorStatus = kMLGenerator.validateFolder();
				if (errorStatus != null) {
					JOptionPane.showMessageDialog(frmKmlGenerator, errorStatus, "Error", JOptionPane.ERROR_MESSAGE);
				} else {
					errorStatus = kMLGenerator.processCSVFiles();
					JOptionPane.showMessageDialog(frmKmlGenerator, errorStatus, "Status", JOptionPane.INFORMATION_MESSAGE);
				}
			}
		});
		

		panel.add(btnNewButton);
	}

}
