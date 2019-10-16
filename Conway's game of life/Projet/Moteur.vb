'Amaury MICHEL
'TP 2I
Public Class Moteur

#Region "Variables"
    Private minMort As Integer
    Private maxMort As Integer
    Private minNaissance As Integer
    Private maxNaissance As Integer
    Private nbLignes As Integer
    Private nbColonnes As Integer
    Private tabEtatCellules(0, 0) As Boolean
    Private tabEtatCellulesCopie(0, 0) As Boolean
#End Region

#Region "Fonctions/Procédures"
    Public Sub setRegle(minMort As Integer, maxMort As Integer, minNaissance As Integer, maxNaissance As Integer)
        MyClass.minMort = minMort
        MyClass.maxMort = maxMort
        MyClass.minNaissance = minNaissance
        MyClass.maxNaissance = maxNaissance
    End Sub

    Public Sub setTailleGrille(n As Integer, m As Integer)
        nbLignes = n
        nbColonnes = m

        'Crée une copie du tableau
        tabEtatCellulesCopie = tabEtatCellules.Clone
        'Redimensionne le tableau
        ReDim tabEtatCellules(nbLignes, nbColonnes)
        'Copie dans le nouveau tableau toutes les cases de l'intersection de l'ancien et du nouveau
        For i As Integer = 0 To Math.Min(tabEtatCellules.GetLength(0), tabEtatCellulesCopie.GetLength(0)) - 1
            For j As Integer = 0 To Math.Min(tabEtatCellules.GetLength(1), tabEtatCellulesCopie.GetLength(1)) - 1
                tabEtatCellules(i, j) = tabEtatCellulesCopie(i, j)
            Next
        Next
    End Sub

    Public Sub getTailleGrille(ByRef n As Integer, ByRef m As Integer)
        n = nbLignes
        m = nbColonnes
    End Sub

    Public Sub setEtatCellule(i As Integer, j As Integer, etat As Boolean)
        tabEtatCellules(i, j) = etat
    End Sub

    Public Function getEtatCellule(i As Integer, j As Integer) As Boolean
        Return tabEtatCellules(i, j)
    End Function

    Public Sub etatSuivant()
        Dim etatCellule As New Integer
        tabEtatCellulesCopie = tabEtatCellules.Clone
        For i As Integer = 0 To nbLignes - 1
            For j As Integer = 0 To nbColonnes - 1
                etatCellule = countEnVie(i, j)
                'Si la cellule est morte et le nombre de cellules en vie autour est compris entre le min et le max de Naissance,
                'la cellule naît
                If (Not tabEtatCellules(i, j) And etatCellule >= minNaissance And etatCellule <= maxNaissance) Then
                    tabEtatCellules(i, j) = True
                    'Si la cellule est en vie et le nombre de cellules en vie autour est inférieur au min de Mort ou 
                    'supérieur au max de Mort, la cellule meurt
                    'On soustrait 1 car si la cellule est en vie, la fonction countEnVie a compté la cellule que l'on change
                ElseIf (tabEtatCellules(i, j) And (etatCellule - 1 < minMort Or etatCellule - 1 > maxMort)) Then
                    tabEtatCellules(i, j) = False
                End If
            Next
        Next
    End Sub

    Private Function countEnVie(i As Integer, j As Integer) As Integer
        Dim count As Integer = 0
        Dim valColonnes As Integer
        Dim valLignes As Integer

        For cpt As Integer = -1 To 1
            'Valeur indiquant la case recherchée
            valLignes = i + cpt
            'Vérifie si la case inspectée est en dehors du tableau et change sa valeur si oui
            If valLignes = -1 Then
                valLignes = nbLignes - 1
            ElseIf valLignes = nbLignes Then
                valLignes = 0
            End If

            For cpt2 As Integer = -1 To 1
                'Valeur indiquant la case recherchée
                valColonnes = j + cpt2
                'Vérifie si la case inspectée est en dehors du tableau et change sa valeur si oui
                If valColonnes = -1 Then
                    valColonnes = nbColonnes - 1
                ElseIf valColonnes = nbColonnes Then
                    valColonnes = 0
                End If

                'Rajoute 1 au compteur si la cellule inspectée est en vie
                'On regarde dans la copie du tableau car le nouveau est mis à jour à chaque fois
                If tabEtatCellulesCopie(valLignes, valColonnes) Then
                    count += 1
                End If
            Next
        Next
        'Retourne le nombre de cellules en vie autour de la cellule que l'on regarde
        Return count
    End Function
#End Region

End Class