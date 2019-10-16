'Amaury MICHEL
'TP 2I
Public Class formIG

#Region "Variables"
    Private frmSize As New Size(600, 650)
    Private nbLignes As Integer
    Private nbColonnes As Integer
    Private pnlList(0, 0) As Panel 'matrice de bouttons

    Private pnl As Panel 'Panel utilisé lors de la création du TableLayoutPanel
    Private tlpList As TableLayoutPanel 'tlp contenant tous les panels
    Private Timer1 As New Timer 'Timer permettant la mise à jour du jeu
    Private tbTick As TrackBar 'TrackBar permettant de changer la vitesse de mise à jour du jeu
    Private lblTick As Label 'Label indiquant la vitesse de mise à jour du jeu

    'Variables fenêtre Aide
    Private btnHelp As Button
    Private frmHelp As Form 'Form du menu Aide
#End Region

#Region "Menu In Game"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DessinerTlpList() 'Desinne le tlp

        Timer1.Interval = 500 'Mets l'intervalle du Timer à 0.5 seconde
        Timer1.Start()
        AddHandler Timer1.Tick, AddressOf PropagationTick

        'Crée le label qui indique le temps entre chaque mise à jour
        lblTick = New Label With {
            .Text = "Temps entre chaque étape : " & (Timer1.Interval / 1000) & " secondes",
            .Location = New Point(btnPause.Right + 100, btnRetour.Top - 30),
            .Anchor = AnchorStyles.Bottom + AnchorStyles.Left + AnchorStyles.Right,
            .AutoSize = True
        }
        MyBase.Controls.Add(lblTick)

        'Crée la TrackBar qui permet de changer le temps entre chaque mise à jour
        tbTick = New TrackBar With {
            .Maximum = 10000,
            .Minimum = 1,
            .Location = New Point(lblTick.Left, btnRetour.Top),
            .Anchor = AnchorStyles.Bottom + AnchorStyles.Left + AnchorStyles.Right,
            .Value = Timer1.Interval,
            .Size = New Drawing.Size(200, 50)
        }
        AddHandler tbTick.ValueChanged, AddressOf changerTick
        MyBase.Controls.Add(tbTick)



    End Sub

    'Redimensionne le TableLayoutPanel
    Private Sub DessinerTlpList()
        'Récupère la taille de la grille
        MoteurUI.moteurInGame.getTailleGrille(nbLignes, nbColonnes)
        'Redimensionne le tableau de panel
        ReDim pnlList(nbLignes, nbColonnes)

        'Retire le tlp de la fenêtre pour ne pas en avoir deux en même temps
        MyBase.Controls.Remove(tlpList)

        'Crée le tlp
        tlpList = New TableLayoutPanel With {
            .Size = New Drawing.Size(500, 500),
            .Margin = New Padding(0),
            .Location = New Point(40, 25),
            .RowCount = nbLignes,
            .ColumnCount = nbColonnes,
            .Anchor = AnchorStyles.Right AndAlso AnchorStyles.Left AndAlso AnchorStyles.Top AndAlso AnchorStyles.Bottom
        }

        'Crée les ColumnStyles, chacun avec la même taille en pourcentage
        For cpt As Integer = 1 To nbColonnes
            tlpList.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, (100 / nbColonnes)))
        Next

        'Crée les RowStyles, chacun avec la même taille en pourcentage
        For cpt As Integer = 1 To nbLignes
            tlpList.RowStyles.Add(New RowStyle(SizeType.Percent, (100 / nbLignes)))
        Next

        'Crée tous les panels et les places dans le tlp et dans le tableau de panels
        For i As Integer = 0 To nbLignes - 1
            For j As Integer = 0 To nbColonnes - 1
                pnl = New Panel With {
                    .Margin = New Padding(0),
                    .Dock = DockStyle.Fill,
                    .BorderStyle = BorderStyle.FixedSingle
                }
                If Not MoteurUI.moteurInGame.getEtatCellule(i, j) Then
                    pnl.BackColor = MoteurUI.pnlCouleurMort.BackColor
                Else
                    pnl.BackColor = MoteurUI.pnlCouleurVie.BackColor
                End If
                AddHandler pnl.Click, AddressOf donnerVie
                pnlList.SetValue(pnl, i, j)
                tlpList.Controls.Add(pnl)
            Next
        Next
        'Ajoute le tlp à la form
        MyBase.Controls.Add(tlpList)
    End Sub

    'S'active lorsque l'on clique sur un des panels
    Public Sub donnerVie(sender As Panel, e As EventArgs)
        'Trouve l'emplacement du panel
        For i As Integer = 0 To nbLignes - 1
            For j As Integer = 0 To nbColonnes - 1
                If pnlList(i, j) Is sender Then
                    'Change l'état de la cellule et sa couleur en fonction de son état précédent
                    If Not MoteurUI.moteurInGame.getEtatCellule(i, j) Then
                        MoteurUI.moteurInGame.setEtatCellule(i, j, True)
                        sender.BackColor = MoteurUI.pnlCouleurVie.BackColor
                    Else
                        MoteurUI.moteurInGame.setEtatCellule(i, j, False)
                        sender.BackColor = MoteurUI.pnlCouleurMort.BackColor
                    End If
                    'Exite la procédure
                    Return
                End If
            Next
        Next
    End Sub

    'Met à jour le jeu
    Private Sub PropagationTick()
        'Active Etat Suivant du moteur
        MoteurUI.moteurInGame.etatSuivant()

        'Change la couleur de chaque panel en fonction de son état
        For i As Integer = 0 To nbLignes - 1
            For j As Integer = 0 To nbColonnes - 1
                If MoteurUI.moteurInGame.getEtatCellule(i, j) Then
                    pnlList(i, j).BackColor = MoteurUI.pnlCouleurVie.BackColor
                Else pnlList(i, j).BackColor = MoteurUI.pnlCouleurMort.BackColor
                End If
            Next
        Next
    End Sub

    'S'active lorsque l'on clique sur le bouton Pause
    Private Sub PlayPause() Handles btnPause.Click
        'Pause le jeu s'il tourne
        If Timer1.Enabled = True Then
            Timer1.Stop()
            btnPause.Text = "Play"
            'Sinon pause le jeu
        Else Timer1.Start()
            btnPause.Text = "Pause"
        End If
    End Sub

    'Procédure qui s'active lorsque l'on change la valeur du TrackBar
    Private Sub changerTick()
        'Change l'interval du Timer
        Timer1.Interval = tbTick.Value
        'Change le texte du label qui indique le temps entre chaque mise à jour
        lblTick.Text = "Temps entre chaque étape : " & (Timer1.Interval / 1000) & " secondes"
    End Sub

    'S'active lorsqu'on clique sur le bouton Retour
    Private Sub retourMenu() Handles btnRetour.Click

        Timer1.Stop() 'Arrête le timer
        btnPause.Text = "Play" 'Change le texte du bouton Pause

        Me.Hide() 'Cache cette fenêtre

        MoteurUI.Controls.Add(MoteurUI.btnHelp) 'Remet le bouton help sur la fenêtre principale
        MoteurUI.numHelp = "1" 'Remet le numéro de la fenêtre Help à 1
        MoteurUI.Show() 'Rouvre le menu principale
    End Sub

    'S'active lorsque l'on ferme cette fenêtre
    Private Sub quitterJeu() Handles MyBase.Closed
        MoteurUI.Close() 'Ferme la fenêtre principale
    End Sub

    'S'active lorsque la fenêtre est à nouveau visible
    Public Sub redessiner() Handles MyBase.VisibleChanged
        MyBase.WindowState = FormWindowState.Normal 'Remet la form en fenetrée
        MyBase.Size = frmSize 'Remet la taille de base
        DessinerTlpList() 'Redessine le tlp avec les nouvelles dimensions
    End Sub
#End Region

End Class

