'Amaury MICHEL
'TP 2I
Public Class MoteurUI

#Region "Variables"

    'Abbréviations :
    'lbl = Label
    'nud = NumericUpDown
    'pnl = Panel
    'btn = Button

#Region "Variables menu principal"
    Private lblTitre As Label

    'Variables nombre de lignes/colonnes
    Private lblNbLignes As Label
    Private lblNbColonnes As Label
    Private nudLignes As NumericUpDown
    Private nudColonnes As NumericUpDown

    'Labels conditions mort/naissance
    Private lblMinMort As Label
    Private lblMaxMort As Label
    Private lblMinNai As Label
    Private lblMaxNai As Label

    'NumericUpDown conditions mort/naissance
    Private nudMinMort As NumericUpDown
    Private nudMaxMort As NumericUpDown
    Private nudMinNai As NumericUpDown
    Private nudMaxNai As NumericUpDown

    'Variables couleurs mort et vie
    Private lblCouleurVie As Label
    Private lblCouleurMort As Label
    Public pnlCouleurMort As Panel
    Public pnlCouleurVie As Panel
    Private colorDlg As New ColorDialog

    'Variables fenêtre Aide
    Public btnHelp As Button
    Private frmHelp As Form
    Public numHelp As Char
    Private srHelp As IO.StreamReader

    'Variables pour changer l'état initial
    Private btnEI As Button 'EI = Etat Initial
    Private frmEI As Form
    Private tlpEI As TableLayoutPanel
    Private pnlListEI(0, 0) As Panel
    Private pnl As Panel
    Private btnConfirmerEI As Button
    Private btnReinitialiserEI As Button

    Private btnLancerPartie As Button


    'Boolean permettant de savoir si la taille de la grille a été changé
    'Cela me permet de ne pas redimensionner la grille si l'utilisateur a changé l'état initial mais n'a pas changé la taille de la grille après
    'car redimensionner le tableau de boolean de la classe moteur réinitialise ses valeurs
    Private blnGrilleChange As Boolean = True


    Public moteurInGame As Moteur
    Private formInGame As Form
    Public dejaOuvert As Boolean
#End Region
#End Region

#Region "Menu principal"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        moteurInGame = New Moteur 'Création du moteur

        numHelp = "1"

        MenuPrincipal() 'Appel de la fonction qui dessine le menu principal
    End Sub

    Private Sub MenuPrincipal()

#Region "Bouton Help et Titre"
        'Bouton aide en haut à gauche de la fenêtre
        btnHelp = New Button With {
            .Text = "?",
            .Size = New Point(20, 20),
            .Location = New Point(5, 5)
        }
        AddHandler btnHelp.Click, AddressOf clickHelp
        MyBase.Controls.Add(btnHelp)

        'Label Jeu de la vie
        lblTitre = New Label With {
            .Size = New Point(235, 50),
            .Text = "Jeu de la vie",
            .Font = New System.Drawing.Font("Berlin Sans FB", 30, System.Drawing.FontStyle.Underline),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        'Les variables sont positionées avec des calculs pour trouver la position exacte
        'Ici la variable est positionée au milieu de la Form
        lblTitre.Location = New Point((MyBase.Width - lblTitre.Width) / 2, 50)
        MyBase.Controls.Add(lblTitre)
#End Region

#Region "Lignes/Colonnes"
        'Label nombre lignes
        lblNbLignes = New Label With {
            .Text = "Nombre de lignes",
            .Size = New Point(155, 30),
            .Font = New System.Drawing.Font("Berlin Sans FB", 15, System.Drawing.FontStyle.Regular),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblNbLignes.Location = New Point((MyBase.Width / 2 - lblNbLignes.Width) / 2, lblTitre.Bottom + 50)
        MyBase.Controls.Add(lblNbLignes)

        'Label nombre colonnes
        lblNbColonnes = New Label With {
            .Text = "Nombre de colonnes",
            .Size = New Point(180, 30),
            .Font = lblNbLignes.Font,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblNbColonnes.Location = New Point((MyBase.Width / 2 - lblNbColonnes.Width + MyBase.Width) / 2, lblTitre.Bottom + 50)
        MyBase.Controls.Add(lblNbColonnes)

        'Combo box pour le nombre de lignes
        nudLignes = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 10,
            .Maximum = 50
        }
        nudLignes.Location = New Point((MyBase.Width / 2 - nudLignes.Width) / 2, lblNbLignes.Bottom + 10)
        AddHandler nudLignes.ValueChanged, AddressOf changerTaille
        MyBase.Controls.Add(nudLignes)

        'Combo box pour le nombre de colonnes
        nudColonnes = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 10,
            .Maximum = 50
        }
        nudColonnes.Location = New Point(nudLignes.Left + MyBase.Width / 2, lblNbLignes.Bottom + 10)
        AddHandler nudColonnes.ValueChanged, AddressOf changerTaille
        MyBase.Controls.Add(nudColonnes)

#End Region

#Region "Mort"
        'Label Minimum Mort
        lblMinMort = New Label With {
            .Text = "Valeur minimale mort",
            .Size = New Point(190, 30),
            .Font = New System.Drawing.Font("Berlin Sans FB", 15, System.Drawing.FontStyle.Regular),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblMinMort.Location = New Point((MyBase.Width / 2 - lblMinMort.Width) / 2, nudColonnes.Bottom + 20)
        MyBase.Controls.Add(lblMinMort)

        'Label Maximum Mort
        lblMaxMort = New Label With {
            .Text = "Valeur maximale mort",
            .Size = New Point(200, 30),
            .Font = lblNbLignes.Font,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblMaxMort.Location = New Point((MyBase.Width / 2 - lblNbColonnes.Width + MyBase.Width) / 2, nudColonnes.Bottom + 20)
        MyBase.Controls.Add(lblMaxMort)

        'NumericUpDown Minimum Mort
        nudMinMort = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 0,
            .Maximum = 8,
            .Value = 2
        }
        nudMinMort.Location = New Point((MyBase.Width / 2 - nudMinMort.Width) / 2, lblMinMort.Bottom + 10)
        MyBase.Controls.Add(nudMinMort)

        'NumericUpDown Maximum Mort
        nudMaxMort = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 0,
            .Maximum = 8,
            .Value = 3
        }
        nudMaxMort.Location = New Point(nudMinMort.Left + MyBase.Width / 2, lblMinMort.Bottom + 10)
        MyBase.Controls.Add(nudMaxMort)
#End Region

#Region "Naissance"
        'Label Minimum Naissance
        lblMinNai = New Label With {
            .Text = "Valeur minimale naissance",
            .Size = New Point(230, 30),
            .Font = New System.Drawing.Font("Berlin Sans FB", 15, System.Drawing.FontStyle.Regular),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblMinNai.Location = New Point((MyBase.Width / 2 - lblMinNai.Width) / 2, nudMinMort.Bottom + 20)
        MyBase.Controls.Add(lblMinNai)

        'Label Maximum Naissance
        lblMaxNai = New Label With {
            .Text = "Valeur maximale naissance",
            .Size = New Point(240, 30),
            .Font = lblNbLignes.Font,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblMaxNai.Location = New Point((MyBase.Width / 2 - lblMaxNai.Width + MyBase.Width) / 2, nudMinMort.Bottom + 20)
        MyBase.Controls.Add(lblMaxNai)

        'NumericUpDown Minimum Naissance
        nudMinNai = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 0,
            .Maximum = 8,
            .Value = 3
        }
        nudMinNai.Location = New Point((MyBase.Width / 2 - nudMinNai.Width) / 2, lblMinNai.Bottom + 10)
        MyBase.Controls.Add(nudMinNai)
        'NumericUpDown Maximum Naissance
        nudMaxNai = New NumericUpDown With {
            .Size = New Size(40, 10),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Minimum = 0,
            .Maximum = 8,
            .Value = 3
        }
        MyBase.Controls.Add(nudMaxNai)
        nudMaxNai.Location = New Point(nudMinNai.Left + MyBase.Width / 2, lblMinNai.Bottom + 10)
#End Region

#Region "Couleur mort/vie"
        'Label couleur mort
        lblCouleurMort = New Label With {
            .Text = "Couleur cellule morte",
            .Size = New Point(190, 30),
            .Font = New System.Drawing.Font("Berlin Sans FB", 15, System.Drawing.FontStyle.Regular),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblCouleurMort.Location = New Point((MyBase.Width / 2 - lblCouleurMort.Width) / 2, nudMaxNai.Bottom + 20)
        MyBase.Controls.Add(lblCouleurMort)

        'Label couleur vie
        lblCouleurVie = New Label With {
            .Text = "Couleur cellule en vie",
            .Size = New Point(195, 30),
            .Font = lblNbLignes.Font,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        lblCouleurVie.Location = New Point(lblCouleurMort.Left + MyBase.Width / 2, nudMaxNai.Bottom + 20)
        MyBase.Controls.Add(lblCouleurVie)

        'NumericUpDown Minimum Mort
        pnlCouleurMort = New Panel With {
            .Size = New Size(20, 20),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .BackColor = Color.White
        }
        pnlCouleurMort.Location = New Point((MyBase.Width / 2 - pnlCouleurMort.Width) / 2, lblCouleurVie.Bottom + 10)
        AddHandler pnlCouleurMort.Click, AddressOf clickCouleurs
        MyBase.Controls.Add(pnlCouleurMort)

        'NumericUpDown Maximum Mort
        pnlCouleurVie = New Panel With {
            .Size = New Size(20, 20),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .BackColor = Color.Black
        }
        pnlCouleurVie.Location = New Point(pnlCouleurMort.Left + MyBase.Width / 2, lblCouleurVie.Bottom + 10)
        AddHandler pnlCouleurVie.Click, AddressOf clickCouleurs
        MyBase.Controls.Add(pnlCouleurVie)
#End Region

#Region "Etat Initial"
        'Création du bouton pour change l'état initial
        btnEI = New Button With {
            .Text = "Changer l'état initial",
            .Size = New Size(120, 23),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        btnEI.Location = New Point((MyBase.Width - btnEI.Width) / 2, pnlCouleurVie.Bottom + 20)
        MyBase.Controls.Add(btnEI)

        AddHandler btnEI.Click, AddressOf changerEI
#End Region

#Region "Bouton Lancer partie"
        'Création du bouton pour lancer la partie
        btnLancerPartie = New Button With {
            .Text = "Lancer la partie",
            .Size = New Size(100, 23),
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        btnLancerPartie.Location = New Point((MyBase.Width - btnLancerPartie.Width) / 2, btnEI.Bottom + 20)
        MyBase.Controls.Add(btnLancerPartie)
        AddHandler btnLancerPartie.Click, AddressOf lancerPartie
#End Region
    End Sub

#Region "Fonctions et procédures"
    'Met le boolean indiquant si la taille de la grille à changée depuis la dernière fois à true
    Private Sub changerTaille()
        blnGrilleChange = True
    End Sub

    'S'active lorsqu'on clique sur le bouton "Lancer la partie"
    Private Sub lancerPartie(sender As Object, e As EventArgs)
        'Paramétrage du moteur
        'Si la taille de la grille a changé, redimensionne la grille du moteur et remettre le boolean à False
        If blnGrilleChange Then
            moteurInGame.setTailleGrille(nudLignes.Value, nudColonnes.Value)
            blnGrilleChange = False
        End If
        moteurInGame.setRegle(nudMinMort.Value, nudMaxMort.Value, nudMinNai.Value, nudMaxNai.Value)

        'Création d'une nouvelle fenêtre si cela n'a pas déjà était fait
        If Not dejaOuvert Then
            formInGame = New formIG()
            dejaOuvert = True
        End If

        formInGame.Controls.Add(btnHelp) 'Met le bouton help sur la form en jeu

        formInGame.Show() 'Affichage de la fenêtre en jeu

        numHelp = "3" 'Remet le numéro de la fenêtre Help à 3

        Me.Hide() 'Cache la fenêtre principale
    End Sub

    'S'active lorsque l'on clique sur un des panels de couleur
    Private Sub clickCouleurs(sender As Object, e As EventArgs)
        'Ouvre une fenêtre de couleur
        If (colorDlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            'Change la couleur du panel
            sender.BackColor = colorDlg.Color
        End If
    End Sub

    'S'active lorsque l'on clique sur le bouton "?"
    Public Sub clickHelp()
        srHelp = IO.File.OpenText("help.txt") 'Ouvre le fichier texte pour le menu aide
        Do 'Cherche la section qui correspond au menu actuel
        Loop While srHelp.ReadLine(0) <> numHelp

        Dim lblText As String = ""
        'Utilise le nombre dans le fichier texte pour savoir le nombre de lignes à ajouter
        For i As Integer = 1 To Integer.Parse(srHelp.ReadLine(0))
            lblText += srHelp.ReadLine + vbNewLine
        Next
        'Ferme le fichier texte
        srHelp.Close()
        'Affiche la fenêtre
        MessageBox.Show(lblText, "Aide")
    End Sub

#Region "Etat Initial"
    'S'active lorsqu'on clique sur le bouton "Changer l'état initial"
    Private Sub changerEI()
        numHelp = "2"

        'Change la taille de la grille et réinitialise le tableau si la valeur des NumericUpDown a changé
        If blnGrilleChange Then
            moteurInGame.setTailleGrille(nudLignes.Value, nudColonnes.Value)
            blnGrilleChange = False
        End If

        ReDim pnlListEI(nudLignes.Value, nudColonnes.Value) 'Redimensionne la liste de panels

        'Crée la forme
        frmEI = New Form With {
            .Size = New Size(315, 375)
        }

        'Crée le tlp de l'état initial
        tlpEI = New TableLayoutPanel With {
            .Size = New Drawing.Size(250, 250),
            .RowCount = nudLignes.Value,
            .ColumnCount = nudColonnes.Value,
            .Margin = New Padding(0),
            .Location = New Point(25, 25),
            .Anchor = AnchorStyles.Right AndAlso AnchorStyles.Left AndAlso AnchorStyles.Top AndAlso AnchorStyles.Bottom
        }

        'Crée les ColumnStyles, chacun avec la même taille en pourcentage
        For cpt As Integer = 1 To nudColonnes.Value
            tlpEI.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, (100 / nudColonnes.Value)))
        Next

        'Crée les RowStyles, chacun avec la même taille en pourcentage
        For cpt As Integer = 1 To nudLignes.Value
            tlpEI.RowStyles.Add(New RowStyle(SizeType.Percent, (100 / nudLignes.Value)))
        Next

        'Crée tous les panels et les places dans le tlp et dans le tableau de panels
        For i As Integer = 0 To nudLignes.Value - 1
            For j As Integer = 0 To nudColonnes.Value - 1
                pnl = New Panel With {
                    .Margin = New Padding(0),
                    .Dock = DockStyle.Fill,
                    .BorderStyle = BorderStyle.FixedSingle
                }
                'Donne la couleur du panel en fonction de son état dans la classe moteur
                If Not moteurInGame.getEtatCellule(i, j) Then
                    pnl.BackColor = pnlCouleurMort.BackColor
                Else
                    pnl.BackColor = pnlCouleurVie.BackColor
                End If
                AddHandler pnl.Click, AddressOf donnerVie
                pnlListEI.SetValue(pnl, i, j)
                tlpEI.Controls.Add(pnl)
            Next
        Next
        frmEI.Controls.Add(tlpEI) 'Ajoute le tlp à la form crée


        'Création du bouton "Confirmer"
        btnConfirmerEI = New Button With {
            .Text = "Confirmer",
            .AutoSize = True,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right,
            .Location = New Point(25, tlpEI.Bottom + 20)
        }
        AddHandler btnConfirmerEI.Click, AddressOf Confirmer
        frmEI.Controls.Add(btnConfirmerEI)

        'Création du bouton "Reinitialiser"
        btnReinitialiserEI = New Button With {
            .Text = "Reinitialiser",
            .AutoSize = True,
            .Anchor = AnchorStyles.Left And AnchorStyles.Right
        }
        btnReinitialiserEI.Location = New Point(tlpEI.Right - btnReinitialiserEI.Width, tlpEI.Bottom + 20)
        AddHandler btnReinitialiserEI.Click, AddressOf Reinitialiser
        frmEI.Controls.Add(btnReinitialiserEI)

        frmEI.Controls.Add(btnHelp)

        'Affichage de la fenêtre crée en Dialog pour empêcher l'utilisateur d'accèder à la fenêtre principale
        frmEI.ShowDialog()
    End Sub

    'S'active lorsque l'on clique sur un des panels
    Public Sub donnerVie(sender As Panel, e As EventArgs)
        'Trouve l'emplacement du panel
        For i As Integer = 0 To nudLignes.Value - 1
            For j As Integer = 0 To nudColonnes.Value - 1
                If pnlListEI(i, j) Is sender Then
                    'Change l'état de la cellule et sa couleur en fonction de son état précédent
                    If Not moteurInGame.getEtatCellule(i, j) Then
                        moteurInGame.setEtatCellule(i, j, True)
                        sender.BackColor = pnlCouleurVie.BackColor
                    Else
                        moteurInGame.setEtatCellule(i, j, False)
                        sender.BackColor = pnlCouleurMort.BackColor
                    End If
                    Return 'Exite la procédure
                End If
            Next
        Next
    End Sub

    'S'active lorsque l'on clique sur le bouton "Confirmer"
    Private Sub Confirmer()
        MyBase.Controls.Add(btnHelp) 'Remet le bouton help sur la fenêtre principale
        numHelp = "1" 'Remet le numéro de la fenêtre Help à 1
        frmEI.Close() 'Fermer le menu Etat Initial
    End Sub

    'S'active lorsque l'on clique sur le bouton "Réinitialiser"
    Private Sub Reinitialiser()
        'Redonne à tous les panels la couleur mort et met leur valeur à False
        For i As Integer = 0 To nudLignes.Value - 1
            For j As Integer = 0 To nudColonnes.Value - 1
                pnlListEI(i, j).BackColor = pnlCouleurMort.BackColor
                moteurInGame.setEtatCellule(i, j, False)
            Next
        Next
    End Sub
#End Region

#End Region

#End Region
End Class